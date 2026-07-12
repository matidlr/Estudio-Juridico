// Models/Usuario.cs
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Ahora hay tres roles: "Cliente", "Abogado", "SuperAdmin"
    public string Rol { get; set; } = "Cliente";

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public Cliente? Cliente { get; set; }
    public Abogado? Abogado { get; set; }
}