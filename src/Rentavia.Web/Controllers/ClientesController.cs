using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rentavia.Application.Services;
using Rentavia.Domain.Entities;
using Rentavia.Infrastructure.Data;
using Rentavia.Infrastructure.Services;
using Rentavia.Web.Models;

namespace Rentavia.Web.Controllers;

public class ClientesController(RentaviaDbContext db, AgendaService agenda) : ProyectoController
{
    public async Task<IActionResult> Index(string? buscar) => View(await db.Clientes.AsNoTracking()
        .Where(c => buscar == null || c.Nombre.Contains(buscar) || c.Apellido.Contains(buscar) || c.NumeroDocumento.Contains(buscar))
        .OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToListAsync());
    public async Task<IActionResult> Editar(int id = 0)
    {
        if (id == 0) return View(new ClienteForm());
        var c = await db.Clientes.FindAsync(id);
        return c is null ? NotFound() : View(new ClienteForm
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            TipoDocumento = c.TipoDocumento,
            NumeroDocumento = c.NumeroDocumento,
            Licencia = c.Licencia,
            VencimientoLicencia = c.VencimientoLicencia,
            Telefono = c.Telefono,
            Correo = c.Correo,
            Activo = c.Activo,
            Version = Convert.ToBase64String(c.Version)
        });
    }
    [HttpPost]
    public async Task<IActionResult> Editar(ClienteForm form)
    {
        if (!ModelState.IsValid) return View(form);
        var c = form.Id == 0 ? new Cliente { FechaRegistroUtc = DateTimeOffset.UtcNow } : await db.Clientes.FindAsync(form.Id);
        if (c is null) return NotFound();
        try
        {
            if (form.Id != 0) ReglasAlquiler.VersionActual(c.Version, Presentacion.Version(form.Version));
            c.Nombre = form.Nombre.Trim(); c.Apellido = form.Apellido.Trim();
            c.TipoDocumento = form.TipoDocumento.Trim().ToUpperInvariant(); c.NumeroDocumento = form.NumeroDocumento.Trim().ToUpperInvariant();
            c.Licencia = form.Licencia.Trim().ToUpperInvariant(); c.VencimientoLicencia = form.VencimientoLicencia;
            c.Telefono = form.Telefono.Trim(); c.Correo = form.Correo?.Trim(); c.Activo = form.Activo;
            if (form.Id == 0) db.Clientes.Add(c);
            await using var tx = await db.Database.BeginTransactionAsync();
            await db.SaveChangesAsync(); agenda.Registrar(Actor, form.Id == 0 ? "Crear" : "Editar", "Cliente", c.Id);
            await db.SaveChangesAsync(); await tx.CommitAsync();
            TempData["Exito"] = "Cliente guardado."; return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is DbUpdateException or ReglaNegocioException) { ErrorFormulario(ex); return View(form); }
    }
    [HttpPost, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id, string version)
    {
        var c = await db.Clientes.FindAsync(id); if (c is null) return NotFound();
        try
        {
            ReglasAlquiler.VersionActual(c.Version, Presentacion.Version(version));
            await using var tx = await db.Database.BeginTransactionAsync();
            if (await db.Reservas.AnyAsync(r => r.ClienteId == id))
            { c.Activo = false; TempData["Exito"] = "Cliente desactivado; se conserva su historial."; }
            else { db.Clientes.Remove(c); TempData["Exito"] = "Cliente eliminado."; }
            agenda.Registrar(Actor, "EliminarODesactivar", "Cliente", id);
            await db.SaveChangesAsync(); await tx.CommitAsync();
        }
        catch (Exception ex) when (ex is DbUpdateException or ReglaNegocioException) { TempData["Error"] = "No se pudo eliminar. Recarga la página o desactiva el cliente."; }
        return RedirectToAction(nameof(Index));
    }
}
