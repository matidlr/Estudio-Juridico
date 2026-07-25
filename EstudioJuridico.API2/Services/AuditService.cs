using Microsoft.AspNetCore.Http;
namespace EstudioJuridico.API2.Services
{
    public class AuditService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db                  = db;
            _httpContextAccessor = httpContextAccessor;
        }

     public async Task Registrar(int usuarioId, string accion, string entidad, int? entidadId = null, string? detalle = null)
{
    try
    {
        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var log = new AuditLog
        {
            UsuarioId = usuarioId,
            Accion    = accion,
            Entidad   = entidad,
            EntidadId = entidadId,
            Detalle   = detalle,
            IpAddress = ip
        };

        _db.AuditLogs.Add(log);
        var filas = await _db.SaveChangesAsync();
        Console.WriteLine($"AuditLog guardado: {accion} en {entidad} - filas: {filas}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en AuditService: {ex.Message}");
        Console.WriteLine($"Inner: {ex.InnerException?.Message}");
    }
}
    }
}