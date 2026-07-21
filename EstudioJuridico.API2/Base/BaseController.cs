using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EstudioJuridico.API2.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected int GetUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return claim != null ? int.Parse(claim) : 0;
        }

        protected string GetRol()
            => User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        protected bool EsSuperAdmin() => GetRol() == "SuperAdmin";
        protected bool EsAbogado() => GetRol() == "Abogado" || EsSuperAdmin();
        protected bool EsCliente() => GetRol() == "Cliente";

        protected IActionResult Exito(object? data = null, string mensaje = "Operación exitosa")
            => Ok(new { success = true, mensaje, data });

        protected IActionResult Error(string mensaje, int codigo = 400)
            => StatusCode(codigo, new { success = false, mensaje });

        protected IActionResult NoEncontrado(string mensaje = "Recurso no encontrado")
            => NotFound(new { success = false, mensaje });

        protected IActionResult SinPermiso(string mensaje = "No tenés permisos para esta acción")
            => StatusCode(403, new { success = false, mensaje });
    }
}