
[ApiController]
[Route("api/abogados")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public class AbogadosController : ControllerBase
{
    private readonly AppDbContext _db;

    public AbogadosController(AppDbContext db)
    {
        _db = db;
    }

[HttpGet]
public async Task<IActionResult> GetTodos()
{
    var count = await _db.Abogados.CountAsync();
    Console.WriteLine($"Total abogados en DB: {count}");
    
    var abogados = await _db.Abogados
        .Include(a => a.Usuario)
        .ToListAsync();

    Console.WriteLine($"Abogados cargados: {abogados.Count}");

    var resultado = abogados.Select(a => new
    {
        a.Id,
        a.Matricula,
        a.Especialidad,
        Nombre   = a.Usuario.Nombre,
        Apellido = a.Usuario.Apellido,
        Email    = a.Usuario.Email,
        Rol      = a.Usuario.Rol
    }).ToList();

    return Ok(resultado);
}

    // GET api/abogados/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var abogado = await _db.Abogados
            .Include(a => a.Usuario)
            .Include(a => a.Casos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (abogado == null)
            return NotFound("Abogado no encontrado.");

        return Ok(new
        {
            abogado.Id,
            abogado.Matricula,
            abogado.Especialidad,
            Nombre   = abogado.Usuario.Nombre,
            Apellido = abogado.Usuario.Apellido,
            Email    = abogado.Usuario.Email,
            Rol      = abogado.Usuario.Rol,
            Casos    = abogado.Casos.Select(c => new
            {
                c.Id,
                c.Caratula,
                c.Tipo,
                c.Estado,
                c.Etapa
            })
        });
    }

    // PUT api/abogados/{id}
    // Solo SuperAdmin puede editar abogados
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Editar(int id, EditarAbogadoDTO dto)
    {
        var abogado = await _db.Abogados
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (abogado == null)
            return NotFound("Abogado no encontrado.");

        abogado.Matricula    = dto.Matricula;
        abogado.Especialidad = dto.Especialidad;
        abogado.Usuario.Nombre   = dto.Nombre;
        abogado.Usuario.Apellido = dto.Apellido;

        await _db.SaveChangesAsync();
        return Ok(new { mensaje = "Abogado actualizado correctamente." });
    }

    // DELETE api/abogados/{id}
    // Solo SuperAdmin puede eliminar abogados
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var abogado = await _db.Abogados
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (abogado == null)
            return NotFound("Abogado no encontrado.");

        // Verificamos que no tenga casos activos
        var tieneCasos = await _db.Casos.AnyAsync(c => c.AbogadoId == id);
        if (tieneCasos)
            return BadRequest("No se puede eliminar un abogado con casos asignados.");

        _db.Abogados.Remove(abogado);
        _db.Usuarios.Remove(abogado.Usuario);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Abogado eliminado correctamente." });
    }
}