[ApiController]
[Route("api/recordatorios")]
[Authorize]
public class RecordatoriosController : ControllerBase
{
    private readonly AppDbContext _db;

    public RecordatoriosController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/recordatorios/caso/{casoId}
    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetRecordatoriosDeCaso(int casoId)
    {
        var recordatorios = await _db.Recordatorios
            .Where(r => r.CasoId == casoId)
            .OrderBy(r => r.FechaEnvio)
            .ToListAsync();

        return Ok(recordatorios);
    }

    // POST api/recordatorios
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CrearRecordatorio(RecordatorioDTO dto)
    {
        var recordatorio = new Recordatorio
        {
            Titulo    = dto.Titulo,
            Mensaje   = dto.Mensaje,
            FechaEnvio = dto.FechaEnvio,
            CasoId    = dto.CasoId
        };

        _db.Recordatorios.Add(recordatorio);
        await _db.SaveChangesAsync();

        return Ok(recordatorio);
    }

    // DELETE api/recordatorios/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarRecordatorio(int id)
    {
        var recordatorio = await _db.Recordatorios.FindAsync(id);
        if (recordatorio == null)
            return NotFound("Recordatorio no encontrado.");

        if (recordatorio.Enviado)
            return BadRequest("No se puede eliminar un recordatorio ya enviado.");

        _db.Recordatorios.Remove(recordatorio);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Recordatorio eliminado correctamente." });
    }

    // GET api/recordatorios
// Trae todos los recordatorios del sistema para el calendario
[HttpGet]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> GetTodos()
{
    var recordatorios = await _db.Recordatorios
        .Include(r => r.Caso)
        .OrderBy(r => r.FechaEnvio)
        .Select(r => new
        {
            r.Id,
            r.Titulo,
            r.Mensaje,
            r.FechaEnvio,
            r.Enviado,
            Caratula = r.Caso.Caratula,
            CasoId   = r.Caso.Id
        })
        .ToListAsync();

    return Ok(recordatorios);
}

// GET api/recordatorios/proximos
// Trae vencimientos y recordatorios de los próximos 30 días
[HttpGet("proximos")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> GetProximos()
{
    var hasta = DateTime.UtcNow.AddDays(30);

    var proximos = await _db.Recordatorios
        .Include(r => r.Caso)
        .Where(r => !r.Enviado && r.FechaEnvio >= DateTime.UtcNow && r.FechaEnvio <= hasta)
        .OrderBy(r => r.FechaEnvio)
        .Select(r => new
        {
            r.Id,
            r.Titulo,
            r.Mensaje,
            r.FechaEnvio,
            r.Tipo,
            r.Enviado,
            Caratula = r.Caso.Caratula,
            CasoId   = r.Caso.Id
        })
        .ToListAsync();

    return Ok(proximos);
}
}