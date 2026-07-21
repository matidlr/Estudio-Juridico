using EstudioJuridico.API2.Base;

public class SeccionExpediente : BaseEntity
{

    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int FojaDesde { get; set; }
    public int FojaHasta { get; set; }
    public int Orden { get; set; } = 0;
    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
    public List<Actualizacion> Actualizaciones { get; set; } = new();
}