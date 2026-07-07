// Models/Comentario.cs
public class Comentario
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // true = el abogado puede verlo, false = es una nota privada del cliente
    public bool VisibleAlAbogado { get; set; } = true;

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}