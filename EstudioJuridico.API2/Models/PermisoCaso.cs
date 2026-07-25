using EstudioJuridico.API2.Base;

public class PermisoCaso : BaseEntity
{
    public int CasoId        { get; set; }
    public int AbogadoId     { get; set; }
    public int OtorgadoPorId { get; set; }

    public Caso    Caso        { get; set; } = null!;
    public Abogado Abogado     { get; set; } = null!;
    public Abogado OtorgadoPor { get; set; } = null!;
}