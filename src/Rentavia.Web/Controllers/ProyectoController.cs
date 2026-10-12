using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rentavia.Application.Services;

namespace Rentavia.Web.Controllers;

[Authorize(Roles = "Administrador,Empleado")]
public abstract class ProyectoController : Controller
{
    protected string Actor => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
    protected void ErrorFormulario(Exception ex) => ModelState.AddModelError("", ex switch
    {
        ReglaNegocioException => ex.Message,
        DbUpdateConcurrencyException => "Otro usuario cambió el registro. Recarga la página.",
        _ => "No se pudo guardar. Revisa duplicados, relaciones o inténtalo nuevamente."
    });
}
