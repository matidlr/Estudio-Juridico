[ApiController]
[Route("api/casos")]
[Authorize]
public class CasosController : ControllerBase
{
    private readonly CasoService _casoService;
    private readonly AppDbContext _db;

    public CasosController(CasoService casoService, AppDbContext db)
    {
        _casoService = casoService;
        _db = db;
    }

    [HttpGet("mios")]
    public async Task<IActionResult> GetMisCasos()
    {
        var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var casos = await _casoService.GetCasosDeCliente(clienteId);
        return Ok(casos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCasoPorId(int id)
    {
        var caso = await _db.Casos
            .Include(c => c.Actualizaciones)
                .ThenInclude(a => a.Archivos)
            .Include(c => c.Archivos)
            .Include(c => c.Pruebas)
            .Include(c => c.Comentarios)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (caso == null)
            return NotFound("Caso no encontrado.");

        return Ok(caso);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetTodosCasos()
    {
        var casos = await _db.Casos
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Usuario)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();

        return Ok(casos);
    }

  [HttpPost]
[Authorize(Roles = "Abogado,SuperAdmin")]
public async Task<IActionResult> CrearCaso(CasoDTO dto)
{
    var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var abogado = await _db.Abogados
        .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);

    if (abogado == null)
        return NotFound("No se encontró el perfil de abogado.");

    var caso = await _casoService.CrearCaso(dto, abogado.Id);
    return Ok(caso);
}

    [HttpPost("actualizacion")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AgregarActualizacion(ActualizacionDTO dto)
    {
        var autorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _casoService.AgregarActualizacion(dto, autorId);
        return Ok(new { mensaje = "Actualización guardada. Cliente notificado." });
    }

    [HttpPost("comentario")]
    public async Task<IActionResult> AgregarComentario(ComentarioDTO dto)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var comentario = new Comentario
        {
            Texto            = dto.Texto,
            CasoId           = dto.CasoId,
            UsuarioId        = usuarioId,
            VisibleAlAbogado = dto.VisibleAlAbogado,
            Fecha            = DateTime.UtcNow
        };

        _db.Comentarios.Add(comentario);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Comentario agregado correctamente." });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> EditarCaso(int id, CasoDTO dto)
    {
        var caso = await _db.Casos.FindAsync(id);
        if (caso == null)
            return NotFound("Caso no encontrado.");

        caso.Titulo       = dto.Titulo;
        caso.NombrePartes = dto.NombrePartes;
        caso.Descripcion  = dto.Descripcion;
        caso.Tipo         = dto.Tipo;
        caso.Estado       = dto.Estado;
        caso.Etapa        = dto.Etapa;

        await _db.SaveChangesAsync();
        return Ok(caso);
    }

   [HttpDelete("{id}")]
[Authorize(Roles = "SuperAdmin")]
public async Task<IActionResult> EliminarCaso(int id)
{
    var caso = await _db.Casos.FindAsync(id);
    if (caso == null)
        return NotFound("Caso no encontrado.");

    _db.Casos.Remove(caso);
    await _db.SaveChangesAsync();

    return Ok(new { mensaje = "Caso eliminado correctamente." });
}

// PUT api/casos/{id}/reasignar
// Solo SuperAdmin puede reasignar el abogado de un caso
[HttpPut("{id}/reasignar")]
[Authorize(Roles = "SuperAdmin")]
public async Task<IActionResult> ReasignarAbogado(int id, [FromBody] ReasignarAbogadoDTO dto)
{
    var caso = await _db.Casos.FindAsync(id);
    if (caso == null)
        return NotFound("Caso no encontrado.");

    var abogado = await _db.Abogados.FindAsync(dto.AbogadoId);
    if (abogado == null)
        return NotFound("Abogado no encontrado.");

    caso.AbogadoId = dto.AbogadoId;
    await _db.SaveChangesAsync();

    return Ok(new { mensaje = "Abogado reasignado correctamente." });
}
}