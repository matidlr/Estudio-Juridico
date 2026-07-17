public class MovimientoDTO
{
    public string Tipo      { get; set; } = string.Empty;
    public string Concepto  { get; set; } = string.Empty;
    public decimal Monto    { get; set; }
    public DateTime Fecha   { get; set; } = DateTime.UtcNow;
    public string? FormaPago { get; set; }
    public string? Notas    { get; set; }
    public int CasoId       { get; set; }
}