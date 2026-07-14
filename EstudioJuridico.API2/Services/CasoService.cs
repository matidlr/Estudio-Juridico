// Services/CasoService.cs
// Lógica de creación, edición y consulta de casos.
public class CasoService
{
    private readonly AppDbContext _db;
    private readonly EmailService _email;
    private readonly WhatsAppService _whatsapp;

    public CasoService(AppDbContext db, EmailService email, WhatsAppService whatsapp)
    {
        _db = db;
        _email = email;
        _whatsapp = whatsapp;
    }

    public async Task<List<Caso>> GetCasosDeCliente(int clienteId)
    {
        return await _db.Casos
            .Include(c => c.Actualizaciones)
            .Include(c => c.Archivos)
            .Where(c => c.ClienteId == clienteId)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();
    }

public async Task<Caso> CrearCaso(CasoDTO dto, int abogadoIdPorDefecto)
{
    var caso = new Caso
    {
        Caratula      = dto.Caratula,
        Proceso       = dto.Proceso,
        Juzgado       = dto.Juzgado,
        NroExpediente = dto.NroExpediente,
        Tipo          = dto.Tipo,
        Estado        = dto.Estado,
        Etapa         = dto.Etapa,
        ClienteId     = dto.ClienteId,
        AbogadoId     = dto.AbogadoId ?? abogadoIdPorDefecto
    };

    _db.Casos.Add(caso);
    await _db.SaveChangesAsync();
    return caso;
}

    public async Task AgregarActualizacion(ActualizacionDTO dto, int autorId)
{
    var actualizacion = new Actualizacion
    {
        Contenido = dto.Contenido,
        CasoId    = dto.CasoId,
        AutorId   = autorId,
        NroFoja   = dto.NroFoja
    };

    _db.Actualizaciones.Add(actualizacion);
    await _db.SaveChangesAsync();

    await NotificarCliente(dto.CasoId);
}

    public async Task NotificarCliente(int casoId)
    {
        // Traemos el caso con los datos del cliente y sus preferencias
        var caso = await _db.Casos
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Preferencias)
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Usuario)
            .FirstOrDefaultAsync(c => c.Id == casoId);

        if (caso?.Cliente?.Preferencias == null) return;

        var prefs = caso.Cliente.Preferencias;
        var usuario = caso.Cliente.Usuario;
        var mensaje = $"Hay una nueva actualización en tu caso: {caso.Caratula}";

        // Solo enviamos por el canal que el cliente eligió Y verificó
        if (prefs.RecibirPorEmail && prefs.EmailConfirmado)
            await _email.Enviar(usuario.Email, "Nueva actualización en tu caso", mensaje);

        if (prefs.RecibirPorWhatsApp && prefs.WhatsAppConfirmado)
            await _whatsapp.Enviar(caso.Cliente.Telefono, mensaje);
    }
}