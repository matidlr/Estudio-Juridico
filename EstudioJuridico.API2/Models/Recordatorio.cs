public class Recordatorio
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public bool Enviado { get; set; } = false;
    public DateTime? FechaEnviado { get; set; }
    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
}