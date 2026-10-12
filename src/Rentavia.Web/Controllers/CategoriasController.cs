using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rentavia.Domain.Entities;
using Rentavia.Infrastructure.Data;
using Rentavia.Infrastructure.Services;
using Rentavia.Web.Models;

namespace Rentavia.Web.Controllers;

public class CategoriasController(RentaviaDbContext db, AgendaService agenda) : ProyectoController
{
    public async Task<IActionResult> Index() => View(await db.Categorias.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync());
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id = 0)
    {
        if (id == 0) return View(new CategoriaForm());
        var c = await db.Categorias.FindAsync(id);
        return c is null ? NotFound() : View(new CategoriaForm { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion });
    }
    [HttpPost, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(CategoriaForm form)
    {
        if (!ModelState.IsValid) return View(form);
        var c = form.Id == 0 ? new Categoria() : await db.Categorias.FindAsync(form.Id);
        if (c is null) return NotFound();
        c.Nombre = form.Nombre.Trim(); c.Descripcion = form.Descripcion?.Trim();
        if (form.Id == 0) db.Categorias.Add(c);
        try
        {
            await using var tx = await db.Database.BeginTransactionAsync();
            await db.SaveChangesAsync(); agenda.Registrar(Actor, form.Id == 0 ? "Crear" : "Editar", "Categoria", c.Id);
            await db.SaveChangesAsync(); await tx.CommitAsync();
            TempData["Exito"] = "Categoría guardada."; return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex) { ErrorFormulario(ex); return View(form); }
    }
    [HttpPost, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var c = await db.Categorias.FindAsync(id); if (c is null) return NotFound();
        try
        {
            await using var tx = await db.Database.BeginTransactionAsync();
            db.Categorias.Remove(c); agenda.Registrar(Actor, "Eliminar", "Categoria", id);
            await db.SaveChangesAsync(); await tx.CommitAsync(); TempData["Exito"] = "Categoría eliminada.";
        }
        catch (DbUpdateException) { TempData["Error"] = "La categoría está referenciada y no puede eliminarse."; }
        return RedirectToAction(nameof(Index));
    }
}
