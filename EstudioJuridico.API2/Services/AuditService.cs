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

                _db.AuditLogs.Add(new AuditLog
                {
                    UsuarioId = usuarioId,
                    Accion    = accion,
                    Entidad   = entidad,
                    EntidadId = entidadId,
                    Detalle   = detalle,
                    IpAddress = ip
                });

                await _db.SaveChangesAsync();
            }
            catch
            {
                // Si falla el log no interrumpimos la operación
            }
        }
    }
}