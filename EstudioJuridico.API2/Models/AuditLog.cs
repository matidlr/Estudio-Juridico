using EstudioJuridico.API2.Base;

public class AuditLog : BaseEntity
{
    public int     UsuarioId { get; set; }
    public string  Accion    { get; set; } = string.Empty;
    public string  Entidad   { get; set; } = string.Empty;
    public int?    EntidadId { get; set; }
    public string? Detalle   { get; set; }
    public string? IpAddress { get; set; }

    public Usuario Usuario { get; set; } = null!;
}