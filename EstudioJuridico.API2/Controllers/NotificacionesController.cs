// Controllers/NotificacionesController.cs
[ApiController]
[Route("api/notificaciones")]
[Authorize]
public class NotificacionesController : ControllerBase
{
    private readonly AppDbContext _db;

    public NotificacionesController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/notificaciones/preferencias
    // El cliente consulta sus preferencias actuales
    [HttpGet("preferencias")]
    public async Task<IActionResult> GetPreferencias()
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var cliente = await _db.Clientes
            .Include(c => c.Preferencias)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (cliente == null)
            return NotFound("Cliente no encontrado.");

        return Ok(cliente.Preferencias);
    }

    // PUT api/notificaciones/preferencias
    // El cliente activa o desactiva email / WhatsApp desde su perfil
    [HttpPut("preferencias")]
    public async Task<IActionResult> ActualizarPreferencias(PreferenciasDTO dto)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var cliente = await _db.Clientes
            .Include(c => c.Preferencias)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (cliente == null)
            return NotFound("Cliente no encontrado.");

        if (cliente.Preferencias == null)
        {
            // Si por alguna razón no tiene preferencias, las creamos
            cliente.Preferencias = new PreferenciasNotificacion
            {
                ClienteId = cliente.Id
            };
            _db.Preferencias.Add(cliente.Preferencias);
        }

        cliente.Preferencias.RecibirPorEmail      = dto.RecibirPorEmail;
        cliente.Preferencias.RecibirPorWhatsApp   = dto.RecibirPorWhatsApp;

        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Preferencias actualizadas correctamente." });
    }
}