// Models/PreferenciasNotificacion.cs
// Guarda si el cliente quiere recibir avisos por email, WhatsApp, o ambos.
public class PreferenciasNotificacion
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public bool RecibirPorEmail { get; set; } = true;
    public bool RecibirPorWhatsApp { get; set; } = false;

    // Solo enviamos si el canal está verificado
    public bool EmailConfirmado { get; set; } = false;
    public bool WhatsAppConfirmado { get; set; } = false;
}