public class RecordatorioDTO
{
    public string Titulo   { get; set; } = string.Empty;
    public string Mensaje  { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public int CasoId     { get; set; }
}