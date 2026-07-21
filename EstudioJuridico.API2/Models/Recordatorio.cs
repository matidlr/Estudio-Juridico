using EstudioJuridico.API2.Base;

public class Recordatorio : BaseEntity
{

    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public bool Enviado { get; set; } = false;
    public DateTime? FechaEnviado { get; set; }
    public string Tipo { get; set; } = "Recordatorio";
    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
}