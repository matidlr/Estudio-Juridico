public class Cliente
{
    public int Id { get; set; }

    // FK hacia Usuario — un cliente siempre tiene un usuario base
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;

    // Navegación: un cliente puede tener muchos casos
    public List<Caso> Casos { get; set; } = new();

    // Preferencias de notificación (email / WhatsApp)
    public PreferenciasNotificacion? Preferencias { get; set; }
}