public class RecordatorioDTO
{
    public string Titulo   { get; set; } = string.Empty;
    public string Mensaje  { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public string Tipo      { get; set; } = "Recordatorio";
    public int CasoId     { get; set; }
}