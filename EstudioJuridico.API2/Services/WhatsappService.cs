using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Services.Interfaces;

public class WhatsAppService : BaseService, IWhatsAppService
{
    private readonly IConfiguration _config;

    public WhatsAppService(IConfiguration config)
    {
        _config = config;
    }

    public async Task Enviar(string numeroDestino, string mensaje)
    {
        ValidarRequerido(numeroDestino, "Número destino");
        ValidarRequerido(mensaje, "Mensaje");

        // Stub — reemplazá con Twilio o Meta API cuando tengas las credenciales
        Console.WriteLine($"[WhatsApp] Para: {numeroDestino} | Mensaje: {mensaje}");
        await Task.CompletedTask;
    }
}