using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/notificaciones")]
[Authorize]
public class NotificacionesController : BaseController
{
    private readonly AppDbContext _db;

    public NotificacionesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("preferencias")]
    public async Task<IActionResult> GetPreferencias()
    {
        var cliente = await _db.Clientes
            .Include(c => c.Preferencias)
            .FirstOrDefaultAsync(c => c.UsuarioId == GetUsuarioId());

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        return Exito(cliente.Preferencias);
    }

    [HttpPut("preferencias")]
    public async Task<IActionResult> ActualizarPreferencias(PreferenciasDTO dto)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Preferencias)
            .FirstOrDefaultAsync(c => c.UsuarioId == GetUsuarioId());

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        if (cliente.Preferencias == null)
        {
            cliente.Preferencias = new PreferenciasNotificacion { ClienteId = cliente.Id };
            _db.Preferencias.Add(cliente.Preferencias);
        }

        cliente.Preferencias.RecibirPorEmail    = dto.RecibirPorEmail;
        cliente.Preferencias.RecibirPorWhatsApp = dto.RecibirPorWhatsApp;

        await _db.SaveChangesAsync();

        return Exito(mensaje: "Preferencias actualizadas correctamente.");
    }
}