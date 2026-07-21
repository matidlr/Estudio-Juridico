using EstudioJuridico.API2.Base;

public class Comentario : BaseEntity
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public bool VisibleAlAbogado { get; set; } = true;
    public string TipoAutor { get; set; } = "Cliente";
    public bool Leida { get; set; } = false;
    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}