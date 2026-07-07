// Models/Abogado.cs
public class Abogado
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string Matricula { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public List<Caso> Casos { get; set; } = new();
}