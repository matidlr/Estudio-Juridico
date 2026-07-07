public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // "Cliente" o "Admin" — define qué puede hacer en el sistema
    public string Rol { get; set; } = "Cliente";

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    // Navegación: si el usuario es cliente, acá vive su perfil extendido
    public Cliente? Cliente { get; set; }
    public Abogado? Abogado { get; set; }
}