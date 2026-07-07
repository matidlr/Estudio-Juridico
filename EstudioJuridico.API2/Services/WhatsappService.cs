// Services/WhatsAppService.cs
public class WhatsAppService
{
    private readonly IConfiguration _config;

    public WhatsAppService(IConfiguration config)
    {
        _config = config;
    }

    public async Task Enviar(string numeroDestino, string mensaje)
    {
        // Por ahora lo dejamos como stub que loguea el mensaje.
        // Cuando tengas la cuenta de Twilio o Meta API, reemplazás
        // el contenido de este método con la llamada real a la API.
        Console.WriteLine($"[WhatsApp] Para: {numeroDestino} | Mensaje: {mensaje}");
        await Task.CompletedTask;
    }
}