using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/recordatorios")]
[Authorize]
public class RecordatoriosController : BaseController
{
    private readonly AppDbContext _db;

    public RecordatoriosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetRecordatoriosDeCaso(int casoId)
    {
        var recordatorios = await _db.Recordatorios
            .Where(r => r.CasoId == casoId)
            .OrderBy(r => r.FechaEnvio)
            .ToListAsync();

        return Exito(recordatorios);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> CrearRecordatorio(RecordatorioDTO dto)
    {
        var recordatorio = new Recordatorio
        {
            Titulo     = dto.Titulo,
            Mensaje    = dto.Mensaje,
            FechaEnvio = dto.FechaEnvio,
            CasoId     = dto.CasoId
        };

        _db.Recordatorios.Add(recordatorio);
        await _db.SaveChangesAsync();

        return Exito(recordatorio, "Recordatorio creado correctamente.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> EliminarRecordatorio(int id)
    {
        var recordatorio = await _db.Recordatorios.FindAsync(id);
        if (recordatorio == null)
            return NoEncontrado("Recordatorio no encontrado.");

        if (recordatorio.Enviado)
            return Error("No se puede eliminar un recordatorio ya enviado.");

        _db.Recordatorios.Remove(recordatorio);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Recordatorio eliminado correctamente.");
    }

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

        return Exito(recordatorios);
    }

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

        return Exito(proximos);
    }
}