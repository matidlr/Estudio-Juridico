using EstudioJuridico.API2.Base;

public class ConsultaPublica : BaseEntity
{
    public string Nombre     { get; set; } = string.Empty;
    public string Email      { get; set; } = string.Empty;
    public string Telefono   { get; set; } = string.Empty;
    public string Mensaje    { get; set; } = string.Empty;
    public bool   Atendida   { get; set; } = false;
    public string? AreaInteres { get; set; }
}