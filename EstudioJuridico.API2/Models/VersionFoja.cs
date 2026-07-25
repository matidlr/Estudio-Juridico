using EstudioJuridico.API2.Base;

public class VersionFoja : BaseEntity
{
    public int    ActualizacionId    { get; set; }
    public string Contenido          { get; set; } = string.Empty;
    public string? NroFoja           { get; set; }
    public string? AclaracionCliente { get; set; }
    public int    Version            { get; set; }
    public int    ModificadoPorId    { get; set; }

    public Actualizacion Actualizacion { get; set; } = null!;
    public Usuario ModificadoPor      { get; set; } = null!;
}