// Services/EmailService.cs
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task Enviar(string destinatario, string asunto, string cuerpo)
    {
        // Leemos la config con valor por defecto para evitar nulls
        var emailFrom    = _config["Email:From"]    ?? throw new InvalidOperationException("Falta Email:From en appsettings.json");
        var emailHost    = _config["Email:Host"]    ?? throw new InvalidOperationException("Falta Email:Host en appsettings.json");
        var emailUser    = _config["Email:User"]    ?? throw new InvalidOperationException("Falta Email:User en appsettings.json");
        var emailPass    = _config["Email:Pass"]    ?? throw new InvalidOperationException("Falta Email:Pass en appsettings.json");
        var emailNombre  = _config["Email:Nombre"]  ?? "Estudio Jurídico";

        var message = new MimeMessage();

        // Ahora emailFrom es string garantizado, no nullable
        message.From.Add(new MailboxAddress(emailNombre, emailFrom));
        message.To.Add(MailboxAddress.Parse(destinatario));
        message.Subject = asunto;

        message.Body = new TextPart("html")
        {
            Text = $@"
                <div style='font-family:sans-serif;padding:20px'>
                    <h2>{emailNombre}</h2>
                    <p>{cuerpo}</p>
                    <p style='color:gray;font-size:12px'>
                        Ingresá a la plataforma para ver los detalles.
                    </p>
                </div>"
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailHost, 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailUser, emailPass);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}