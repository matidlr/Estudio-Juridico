public class RecordatorioService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<RecordatorioService> _logger;

    public RecordatorioService(
        IServiceProvider services,
        ILogger<RecordatorioService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcesarRecordatorios();

            // Revisa cada 60 segundos
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }
    }

    private async Task ProcesarRecordatorios()
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

        // Buscamos recordatorios pendientes cuya fecha ya pasó
        var recordatoriosPendientes = await db.Recordatorios
            .Include(r => r.Caso)
                .ThenInclude(c => c.Cliente)
                    .ThenInclude(cl => cl.Usuario)
            .Include(r => r.Caso)
                .ThenInclude(c => c.Cliente)
                    .ThenInclude(cl => cl.Preferencias)
            .Where(r => !r.Enviado && r.FechaEnvio <= DateTime.UtcNow)
            .ToListAsync();

        foreach (var recordatorio in recordatoriosPendientes)
        {
            try
            {
                var cliente = recordatorio.Caso.Cliente;
                var prefs   = cliente.Preferencias;
                var email   = cliente.Usuario.Email;
                var asunto  = $"Recordatorio: {recordatorio.Titulo}";
                var cuerpo  = $@"
                    <h2>{recordatorio.Titulo}</h2>
                    <p>{recordatorio.Mensaje}</p>
                    <p>Caso: <strong>{recordatorio.Caso.Titulo}</strong></p>
                    <p>Fecha: {recordatorio.FechaEnvio:dd/MM/yyyy HH:mm}</p>
                ";

                if (prefs == null || prefs.RecibirPorEmail)
                    await emailService.Enviar(email, asunto, cuerpo);

                recordatorio.Enviado     = true;
                recordatorio.FechaEnviado = DateTime.UtcNow;

                _logger.LogInformation(
                    "Recordatorio {Id} enviado a {Email}", recordatorio.Id, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar recordatorio {Id}", recordatorio.Id);
            }
        }

        await db.SaveChangesAsync();
    }
}