// Models/Prueba.cs
using EstudioJuridico.API2.Base;

public class Prueba : BaseEntity
{

    public string Descripcion { get; set; } = string.Empty;
    public string UrlArchivo { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public DateTime FechaCarga { get; set; } = DateTime.UtcNow;

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
}