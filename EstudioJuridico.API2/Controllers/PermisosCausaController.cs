using EstudioJuridico.API2.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/permisos-causa")]
[Authorize(Roles = "Abogado,SuperAdmin")]
public class PermisosCausaController : BaseController
{
    private readonly AppDbContext _db;

    public PermisosCausaController(AppDbContext db)
    {
        _db = db;
    }

    // Obtener permisos de una causa
    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetPermisos(int casoId)
    {
        var permisos = await _db.PermisosCausa
            .Include(p => p.Abogado).ThenInclude(a => a.Usuario)
            .Include(p => p.OtorgadoPor).ThenInclude(a => a.Usuario)
            .Where(p => p.CasoId == casoId)
            .Select(p => new
            {
                p.Id,
                p.CasoId,
                Abogado = p.Abogado.Usuario.Nombre + " " + p.Abogado.Usuario.Apellido,
                p.AbogadoId,
                OtorgadoPor = p.OtorgadoPor.Usuario.Nombre + " " + p.OtorgadoPor.Usuario.Apellido,
                p.CreadoEn
            })
            .ToListAsync();

        return Exito(permisos);
    }

    // Dar permiso a un abogado
    [HttpPost]
    public async Task<IActionResult> DarPermiso([FromBody] PermisoCasoDTO dto)
    {
        var abogadoActual = await _db.Abogados
            .FirstOrDefaultAsync(a => a.UsuarioId == GetUsuarioId());

        if (abogadoActual == null)
            return Error("No se encontró el perfil de abogado.");

        // Verificamos que sea el titular o SuperAdmin
        var caso = await _db.Casos.FindAsync(dto.CasoId);
        if (caso == null)
            return NoEncontrado("Caso no encontrado.");

        if (GetRol() != "SuperAdmin" && caso.AbogadoId != abogadoActual.Id)
            return Error("Solo el abogado titular puede delegar permisos.", 403);

        // Verificamos que no exista ya el permiso
        var yaExiste = await _db.PermisosCausa
            .AnyAsync(p => p.CasoId == dto.CasoId && p.AbogadoId == dto.AbogadoId);

        if (yaExiste)
            return Error("Este abogado ya tiene permiso en esta causa.");

        var permiso = new PermisoCaso
        {
            CasoId        = dto.CasoId,
            AbogadoId     = dto.AbogadoId,
            OtorgadoPorId = abogadoActual.Id
        };

        _db.PermisosCausa.Add(permiso);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Permiso otorgado correctamente.");
    }

    // Revocar permiso
    [HttpDelete("{id}")]
    public async Task<IActionResult> RevocarPermiso(int id)
    {
        var permiso = await _db.PermisosCausa.FindAsync(id);
        if (permiso == null)
            return NoEncontrado("Permiso no encontrado.");

        var abogadoActual = await _db.Abogados
            .FirstOrDefaultAsync(a => a.UsuarioId == GetUsuarioId());

        var caso = await _db.Casos.FindAsync(permiso.CasoId);

        if (GetRol() != "SuperAdmin" && caso?.AbogadoId != abogadoActual?.Id)
            return Error("Solo el abogado titular puede revocar permisos.", 403);

        _db.PermisosCausa.Remove(permiso);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Permiso revocado correctamente.");
    }
}