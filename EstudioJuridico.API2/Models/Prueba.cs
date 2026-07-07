// Models/Prueba.cs
public class Prueba
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string UrlArchivo { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public DateTime FechaCarga { get; set; } = DateTime.UtcNow;

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
}