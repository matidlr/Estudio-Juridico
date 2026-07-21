using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/abogados")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public class AbogadosController : BaseController  // ← cambio acá
{
    private readonly AppDbContext _db;

    public AbogadosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var abogados = await _db.Abogados
            .Include(a => a.Usuario)
            .ToListAsync();

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

        return Exito(resultado);  // ← usás Exito() en vez de Ok()
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var abogado = await _db.Abogados
            .Include(a => a.Usuario)
            .Include(a => a.Casos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (abogado == null)
            return NoEncontrado("Abogado no encontrado.");  // ← NoEncontrado()

        return Exito(new
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

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Editar(int id, EditarAbogadoDTO dto)
    {
        var abogado = await _db.Abogados
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (abogado == null)
            return NoEncontrado("Abogado no encontrado.");

        abogado.Matricula        = dto.Matricula;
        abogado.Especialidad     = dto.Especialidad;
        abogado.Usuario.Nombre   = dto.Nombre;
        abogado.Usuario.Apellido = dto.Apellido;

        await _db.SaveChangesAsync();
        return Exito(mensaje: "Abogado actualizado correctamente.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var abogado = await _db.Abogados
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (abogado == null)
            return NoEncontrado("Abogado no encontrado.");

        var tieneCasos = await _db.Casos.AnyAsync(c => c.AbogadoId == id);
        if (tieneCasos)
            return Error("No se puede eliminar un abogado con casos asignados.");

        _db.Abogados.Remove(abogado);
        _db.Usuarios.Remove(abogado.Usuario);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Abogado eliminado correctamente.");
    }
}