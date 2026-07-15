[ApiController]
[Route("api/casos")]
[Authorize]
public class CasosController : ControllerBase
{
    private readonly CasoService _casoService;
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

public CasosController(CasoService casoService, AppDbContext db, IWebHostEnvironment env)
{
    _casoService = casoService;
    _db = db;
    _env = env;
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
        .Include(c => c.Abogado)
            .ThenInclude(a => a.Usuario)
        .OrderByDescending(c => c.FechaInicio)
        .ToListAsync();

    var resultado = casos.Select(c => new
    {
        c.Id,
        c.Caratula,
        c.Proceso,
        c.Juzgado,
        c.NroExpediente,
        c.Tipo,
        c.Estado,
        c.Etapa,
        c.FechaInicio,
        c.AbogadoId,
        c.ClienteId,
        Cliente = c.Cliente == null ? null : new
        {
            c.Cliente.Id,
            Nombre   = c.Cliente.Usuario?.Nombre,
            Apellido = c.Cliente.Usuario?.Apellido,
            Email    = c.Cliente.Usuario?.Email
        },
        Abogado = c.Abogado == null ? null : new
        {
            c.Abogado.Id,
            Nombre   = c.Abogado.Usuario?.Nombre,
            Apellido = c.Abogado.Usuario?.Apellido
        }
    }).ToList();

    return Ok(resultado);
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
                caso.Caratula      = dto.Caratula;
                caso.Proceso       = dto.Proceso;
                caso.Juzgado       = dto.Juzgado;
                caso.NroExpediente = dto.NroExpediente;
                caso.Tipo          = dto.Tipo;
                caso.Estado        = dto.Estado;
                caso.Etapa         = dto.Etapa;
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

// POST api/casos/actualizacion-con-archivo
[HttpPost("actualizacion-con-archivo")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> AgregarActualizacionConArchivo(
    [FromForm] string contenido,
    [FromForm] int casoId,
    [FromForm] string? nroFoja,
    [FromForm] IFormFile? archivo)
{
    var autorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var actualizacion = new Actualizacion
    {
        Contenido = contenido,
        CasoId    = casoId,
        AutorId   = autorId,
        NroFoja   = nroFoja
    };

    _db.Actualizaciones.Add(actualizacion);
    await _db.SaveChangesAsync();

    // Si hay archivo lo guardamos
    if (archivo != null && archivo.Length > 0)
    {
        var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".txt", ".docx" };
        var extension = Path.GetExtension(archivo.FileName).ToLower();

        if (extensionesPermitidas.Contains(extension))
        {
            var carpeta = Path.Combine(_env.WebRootPath, "uploads", "casos", casoId.ToString(), "fojas");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaCompleta  = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var url = $"/uploads/casos/{casoId}/fojas/{nombreArchivo}";
            var nuevoArchivo = new Archivo
            {
                Nombre          = archivo.FileName,
                Tipo            = extension.Replace(".", "").ToUpper(),
                Categoria       = "Foja",
                Url             = url,
                CasoId          = casoId,
                ActualizacionId = actualizacion.Id
            };

            _db.Archivos.Add(nuevoArchivo);
            await _db.SaveChangesAsync();
        }
    }

    // Notificamos al cliente
    await _casoService.NotificarCliente(casoId);

    return Ok(new { mensaje = "Foja agregada correctamente." });
}
}