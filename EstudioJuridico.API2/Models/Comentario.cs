public class Comentario
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public bool VisibleAlAbogado { get; set; } = true;
    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    // Para saber si es respuesta del abogado o consulta del cliente
    public string TipoAutor { get; set; } = "Cliente";
}