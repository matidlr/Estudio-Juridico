// Models/Archivo.cs
using EstudioJuridico.API2.Base;

public class Archivo : BaseEntity
{

    public string Nombre { get; set; } = string.Empty;

    // "PDF", "Imagen", "Texto"
    public string Tipo { get; set; } = string.Empty;

    // "Documento" o "Prueba"
    public string Categoria { get; set; } = "Documento";

    // URL donde está guardado el archivo (carpeta local o servicio en la nube)
    public string Url { get; set; } = string.Empty;

    public DateTime SubidoEn { get; set; } = DateTime.UtcNow;

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;

    // Puede estar adjunto a una actualización específica (opcional)
    public int? ActualizacionId { get; set; }
    public Actualizacion? Actualizacion { get; set; }
    public int? SeccionExpedienteId { get; set; }
    public SeccionExpediente? SeccionExpediente { get; set; }
}