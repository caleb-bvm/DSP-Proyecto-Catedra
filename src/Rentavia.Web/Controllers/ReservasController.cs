using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rentavia.Application.Services;
using Rentavia.Domain.Entities;
using Rentavia.Infrastructure.Data;
using Rentavia.Infrastructure.Services;
using Rentavia.Web.Models;

namespace Rentavia.Web.Controllers;

public class ReservasController(RentaviaDbContext db, AgendaService agenda) : ProyectoController
{
    public async Task<IActionResult> Index() => View(await db.Reservas.AsNoTracking().Include(r => r.Cliente).Include(r => r.Vehiculo).OrderByDescending(r => r.CreadaUtc).ToListAsync());
    public async Task<IActionResult> Editar(int id = 0, int vehiculoId = 0, DateTime? inicio = null, DateTime? fin = null)
    {
        await OpcionesAsync();
        if (id == 0) return View(new ReservaForm { VehiculoId = vehiculoId, Inicio = inicio ?? Presentacion.Local(DateTimeOffset.UtcNow), Fin = fin ?? Presentacion.Local(DateTimeOffset.UtcNow).AddDays(1) });
        var r = await db.Reservas.FindAsync(id);
        return r is null ? NotFound() : View(new ReservaForm
        {
            Id = r.Id,
            ClienteId = r.ClienteId,
            VehiculoId = r.VehiculoId,
            Inicio = Presentacion.Local(r.InicioUtc),
            Fin = Presentacion.Local(r.FinUtc),
            Version = Convert.ToBase64String(r.Version)
        });
    }
    [HttpPost]
    public async Task<IActionResult> Editar(ReservaForm form)
    {
        await OpcionesAsync();
        if (!ModelState.IsValid) return View(form);
        try
        {
            await agenda.GuardarReservaAsync(form.Id == 0 ? null : form.Id, form.ClienteId, form.VehiculoId,
                Presentacion.Utc(form.Inicio), Presentacion.Utc(form.Fin), Presentacion.Version(form.Version), Actor);
            TempData["Exito"] = "Reserva confirmada."; return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is ReglaNegocioException or DbUpdateException) { ErrorFormulario(ex); return View(form); }
    }
    [HttpPost]
    public async Task<IActionResult> Cancelar(int id, string version)
    {
        try { await agenda.CancelarReservaAsync(id, Presentacion.Version(version), Actor); TempData["Exito"] = "Reserva cancelada."; }
        catch (Exception ex) when (ex is ReglaNegocioException or DbUpdateException) { TempData["Error"] = ex is ReglaNegocioException ? ex.Message : "No se pudo cancelar. Recarga la página."; }
        return RedirectToAction(nameof(Index));
    }
    private async Task OpcionesAsync()
    {
        var clientes = await db.Clientes.Where(x => x.Activo).OrderBy(x => x.Nombre).Select(x => new { x.Id, Nombre = x.Nombre + " " + x.Apellido }).ToListAsync();
        var vehiculos = await db.Vehiculos.OrderBy(x => x.Placa).Select(x => new { x.Id, Nombre = x.Placa + " - " + x.Marca + " " + x.Modelo }).ToListAsync();
        ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre"); ViewBag.Vehiculos = new SelectList(vehiculos, "Id", "Nombre");
    }
}
