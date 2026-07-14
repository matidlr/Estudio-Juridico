// Models/Actualizacion.cs
// Cada novedad que el abogado carga en el caso.
public class Actualizacion
{
    public int Id { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string? NroFoja { get; set; }
    public string? AclaracionCliente { get; set; }

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;

    // Quién la escribió (siempre el abogado)
    public int AutorId { get; set; }
    public Usuario Autor { get; set; } = null!;

    public List<Archivo> Archivos { get; set; } = new();
}