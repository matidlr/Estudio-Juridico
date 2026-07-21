public class SeccionExpedienteDTO
{
    public string Titulo       { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int FojaDesde       { get; set; }
    public int FojaHasta       { get; set; }
    public int Orden           { get; set; }
    public int CasoId          { get; set; }
}dotbet