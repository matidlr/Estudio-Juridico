namespace EstudioJuridico.API2.Events
{
    public class FojaAgregadaEvent
    {
        public int CasoId { get; set; }
        public string Caratula { get; set; } = string.Empty;
        public string? NroFoja { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}