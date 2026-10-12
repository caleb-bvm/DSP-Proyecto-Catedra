using System.Data;
using Microsoft.EntityFrameworkCore;
using Rentavia.Application.Services;
using Rentavia.Domain.Entities;
using Rentavia.Infrastructure.Data;

namespace Rentavia.Infrastructure.Services;

public class AgendaService(RentaviaDbContext db)
{
    public IQueryable<Vehiculo> Disponibles(DateTimeOffset inicio, DateTimeOffset fin, int? excluirReserva = null)
    {
        ReglasAlquiler.Dias(inicio, fin);
        return db.Vehiculos.AsNoTracking().Include(x => x.Categoria).Include(x => x.Sucursal)
            .Where(v => v.Estado == EstadoVehiculo.Activo && v.Sucursal.Activa && v.TarifaDiaria > 0)
            .Where(v => !db.Ocupaciones.Any(o => o.VehiculoId == v.Id && o.Activa
                && (excluirReserva == null || o.ReservaId != excluirReserva)
                && o.InicioUtc < fin && inicio < o.FinUtc))
            // Un alquiler vencido todavía abierto requiere intervención; no ofrecer el vehículo.
            .Where(v => !db.Alquileres.Any(a => a.VehiculoId == v.Id
                && a.Estado == EstadoAlquiler.Abierto && a.Reserva.FinUtc < DateTimeOffset.UtcNow))
            .Where(v => !db.Mantenimientos.Any(m => m.VehiculoId == v.Id
                && !m.Finalizado && m.FinUtc < DateTimeOffset.UtcNow));
    }

    public async Task<Reserva> GuardarReservaAsync(int? id, int clienteId, int vehiculoId,
        DateTimeOffset inicio, DateTimeOffset fin, byte[] version, string actor)
    {
        var dias = ReglasAlquiler.Dias(inicio, fin);
        if (inicio < DateTimeOffset.UtcNow.AddMinutes(-5))
            throw new ReglaNegocioException("La reserva no puede comenzar en el pasado.");
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        // Bloqueamos el vehículo hasta confirmar: dos empleados no reservan el mismo intervalo.
        var vehiculo = await BloquearVehiculoAsync(vehiculoId);
        if (vehiculo.Estado != EstadoVehiculo.Activo || !await db.Sucursales.AnyAsync(s => s.Id == vehiculo.SucursalId && s.Activa))
            throw new ReglaNegocioException("El vehículo o su sucursal no están activos.");
        if (!await db.Clientes.AnyAsync(c => c.Id == clienteId && c.Activo && c.VencimientoLicencia >= DateOnly.FromDateTime(fin.UtcDateTime.AddHours(-6))))
            throw new ReglaNegocioException("Selecciona un cliente activo con licencia vigente durante la reserva.");
        var reserva = id.HasValue ? await db.Reservas.FindAsync(id.Value) : new Reserva();
        if (reserva is null) throw new ReglaNegocioException("La reserva no existe.");
        if (id.HasValue)
        {
            ReglasAlquiler.VersionActual(reserva.Version, version);
            if (reserva.Estado != EstadoReserva.Confirmada || reserva.VehiculoId != vehiculoId)
                throw new ReglaNegocioException("Solo se edita una reserva confirmada del mismo vehículo. Cancela y crea otra para cambiarlo.");
        }
        await ComprobarLibreAsync(vehiculoId, inicio, fin, id);
        reserva.ClienteId = clienteId;
        reserva.VehiculoId = vehiculoId;
        reserva.InicioUtc = inicio;
        reserva.FinUtc = fin;
        // Al editar fechas conservamos la tarifa histórica.
        if (!id.HasValue) reserva.TarifaDiaria = vehiculo.TarifaDiaria;
        reserva.Dias = dias;
        reserva.TotalEstimado = ReglasAlquiler.Total(inicio, fin, reserva.TarifaDiaria);
        if (!id.HasValue) db.Reservas.Add(reserva);
        var ocupacion = id.HasValue ? await db.Ocupaciones.SingleAsync(o => o.ReservaId == id) : new Ocupacion { Reserva = reserva, VehiculoId = vehiculoId };
        ocupacion.InicioUtc = inicio;
        ocupacion.FinUtc = fin;
        if (!id.HasValue) db.Ocupaciones.Add(ocupacion);
        await db.SaveChangesAsync();
        Registrar(actor, id.HasValue ? "Editar" : "Crear", "Reserva", reserva.Id);
        await db.SaveChangesAsync();
        await tx.CommitAsync();
        return reserva;
    }

    public async Task CancelarReservaAsync(int id, byte[] version, string actor)
    {
        var vehiculoId = await db.Reservas.AsNoTracking().Where(r => r.Id == id).Select(r => (int?)r.VehiculoId).SingleOrDefaultAsync()
            ?? throw new ReglaNegocioException("La reserva no existe.");
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        await BloquearVehiculoAsync(vehiculoId);
        var reserva = await db.Reservas.SingleAsync(r => r.Id == id);
        ReglasAlquiler.VersionActual(reserva.Version, version);
        if (reserva.Estado != EstadoReserva.Confirmada) throw new ReglaNegocioException("Solo se cancela una reserva confirmada.");
        reserva.Estado = EstadoReserva.Cancelada;
        (await db.Ocupaciones.SingleAsync(o => o.ReservaId == id)).Activa = false;
        Registrar(actor, "Cancelar", "Reserva", id);
        await db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task<Mantenimiento> ProgramarMantenimientoAsync(int vehiculoId, DateTimeOffset inicio,
        DateTimeOffset fin, string tipo, string descripcion, decimal costo, string actor)
    {
        ReglasAlquiler.Dias(inicio, fin);
        if (inicio < DateTimeOffset.UtcNow.AddMinutes(-5) || costo < 0 || costo > 999999)
            throw new ReglaNegocioException("Revisa las fechas y el costo del mantenimiento.");
        if (tipo is not ("Preventivo" or "Correctivo") || string.IsNullOrWhiteSpace(descripcion) || descripcion.Length > 500)
            throw new ReglaNegocioException("Indica tipo y descripción del mantenimiento.");
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var vehiculo = await BloquearVehiculoAsync(vehiculoId);
        if (vehiculo.Estado == EstadoVehiculo.Retirado) throw new ReglaNegocioException("El vehículo está retirado.");
        await ComprobarLibreAsync(vehiculoId, inicio, fin);
        var mantenimiento = new Mantenimiento
        {
            VehiculoId = vehiculoId,
            InicioUtc = inicio,
            FinUtc = fin,
            Tipo = tipo,
            Descripcion = descripcion.Trim(),
            Costo = costo
        };
        db.Mantenimientos.Add(mantenimiento);
        db.Ocupaciones.Add(new Ocupacion { VehiculoId = vehiculoId, Mantenimiento = mantenimiento, InicioUtc = inicio, FinUtc = fin });
        await db.SaveChangesAsync();
        Registrar(actor, "Programar", "Mantenimiento", mantenimiento.Id);
        await db.SaveChangesAsync();
        await tx.CommitAsync();
        return mantenimiento;
    }

    public async Task FinalizarMantenimientoAsync(int id, string actor)
    {
        var vehiculoId = await db.Mantenimientos.AsNoTracking().Where(m => m.Id == id).Select(m => (int?)m.VehiculoId).SingleOrDefaultAsync()
            ?? throw new ReglaNegocioException("El mantenimiento no existe.");
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        await BloquearVehiculoAsync(vehiculoId);
        var mantenimiento = await db.Mantenimientos.SingleAsync(m => m.Id == id);
        if (mantenimiento.Finalizado) throw new ReglaNegocioException("El mantenimiento ya finalizó.");
        mantenimiento.Finalizado = true;
        (await db.Ocupaciones.SingleAsync(o => o.MantenimientoId == id)).Activa = false;
        Registrar(actor, "Finalizar", "Mantenimiento", id);
        await db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    internal async Task<Vehiculo> BloquearVehiculoAsync(int id) => await db.Vehiculos
        .FromSqlInterpolated($"SELECT * FROM Vehiculos WITH (UPDLOCK, HOLDLOCK) WHERE Id = {id}").SingleOrDefaultAsync()
        ?? throw new ReglaNegocioException("El vehículo no existe.");

    internal async Task ComprobarLibreAsync(int vehiculoId, DateTimeOffset inicio, DateTimeOffset fin, int? excluirReserva = null)
    {
        if (await db.Ocupaciones.AnyAsync(o => o.VehiculoId == vehiculoId && o.Activa
            && (excluirReserva == null || o.ReservaId != excluirReserva)
            && o.InicioUtc < fin && inicio < o.FinUtc))
            throw new ReglaNegocioException("El vehículo ya tiene una reserva, alquiler o mantenimiento en ese período.");
        if (await db.Alquileres.AnyAsync(a => a.VehiculoId == vehiculoId && a.Estado == EstadoAlquiler.Abierto && a.Reserva.FinUtc < DateTimeOffset.UtcNow))
            throw new ReglaNegocioException("El vehículo tiene una devolución pendiente; resuélvela antes de programar otra operación.");
        if (await db.Mantenimientos.AnyAsync(m => m.VehiculoId == vehiculoId && !m.Finalizado && m.FinUtc < DateTimeOffset.UtcNow))
            throw new ReglaNegocioException("El vehículo tiene mantenimiento pendiente de finalizar.");
    }

    public void Registrar(string actor, string accion, string entidad, int id, string? clave = null) =>
        db.Auditorias.Add(new Auditoria { UsuarioId = actor, Accion = accion, Entidad = entidad, RegistroId = id, RegistroClave = clave });
}
