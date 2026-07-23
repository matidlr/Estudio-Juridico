using EstudioJuridico.API2.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/consultas-publicas")]
public class ConsultasPublicasController : BaseController
{
    private readonly AppDbContext _db;

    public ConsultasPublicasController(AppDbContext db)
    {
        _db = db;
    }

    // POST api/consultas-publicas → público, sin autenticación
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Crear([FromBody] ConsultaPublicaDTO dto)
    {
        var consulta = new ConsultaPublica
        {
            Nombre      = dto.Nombre,
            Email       = dto.Email,
            Telefono    = dto.Telefono,
            Mensaje     = dto.Mensaje,
            AreaInteres = dto.AreaInteres
        };

        _db.ConsultasPublicas.Add(consulta);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Consulta enviada correctamente. Nos comunicaremos a la brevedad.");
    }

    // GET api/consultas-publicas → solo abogados
    [HttpGet]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetTodas()
    {
        var consultas = await _db.ConsultasPublicas
            .OrderByDescending(c => c.CreadoEn)
            .ToListAsync();

        return Exito(consultas);
    }

    // PUT api/consultas-publicas/{id}/atendida
    [HttpPut("{id}/atendida")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> MarcarAtendida(int id)
    {
        var consulta = await _db.ConsultasPublicas.FindAsync(id);
        if (consulta == null)
            return NoEncontrado("Consulta no encontrada.");

        consulta.Atendida = !consulta.Atendida;
        await _db.SaveChangesAsync();

        return Exito(mensaje: consulta.Atendida ? "Marcada como atendida." : "Marcada como pendiente.");
    }

    // DELETE api/consultas-publicas/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var consulta = await _db.ConsultasPublicas.FindAsync(id);
        if (consulta == null)
            return NoEncontrado("Consulta no encontrada.");

        _db.ConsultasPublicas.Remove(consulta);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Consulta eliminada correctamente.");
    }
}