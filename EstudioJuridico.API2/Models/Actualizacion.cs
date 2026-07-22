using EstudioJuridico.API2.Base;
public class Actualizacion : BaseEntity
{
    public string Contenido { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string? NroFoja { get; set; }
    public string? AclaracionCliente { get; set; }

   
    public int? SeccionExpedienteId { get; set; }
    public SeccionExpediente? SeccionExpediente { get; set; }

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;

    public int AutorId { get; set; }
    public Usuario Autor { get; set; } = null!;

    public List<Archivo> Archivos { get; set; } = new();
}