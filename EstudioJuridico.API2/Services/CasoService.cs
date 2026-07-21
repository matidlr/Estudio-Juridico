using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Services.Interfaces;

public class CasoService : BaseService, ICasoService
{
    private readonly AppDbContext _db;
    private readonly IEstudioEmailService _email;
    private readonly IWhatsAppService _whatsapp;  // ← interfaz

   public CasoService(AppDbContext db, IEstudioEmailService email, IWhatsAppService whatsapp)
    {
        _db       = db;
        _email    = email;
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
        ValidarRequerido(dto.Caratula, "Carátula");
        ValidarRequerido(dto.Tipo, "Tipo");

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
        ValidarRequerido(dto.Contenido, "Contenido");

        var actualizacion = new Actualizacion
        {
            Contenido         = dto.Contenido,
            CasoId            = dto.CasoId,
            AutorId           = autorId,
            NroFoja           = dto.NroFoja,
            AclaracionCliente = dto.AclaracionCliente
        };

        _db.Actualizaciones.Add(actualizacion);
        await _db.SaveChangesAsync();

        await NotificarCliente(dto.CasoId);
    }

    public async Task NotificarCliente(int casoId)
    {
        var caso = await _db.Casos
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Preferencias)
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Usuario)
            .FirstOrDefaultAsync(c => c.Id == casoId);

        if (caso?.Cliente?.Preferencias == null) return;

        var prefs   = caso.Cliente.Preferencias;
        var usuario = caso.Cliente.Usuario;
        var mensaje = $"Hay una nueva actualización en tu caso: {caso.Caratula}";

        if (prefs.RecibirPorEmail && prefs.EmailConfirmado)
            await _email.Enviar(usuario.Email, "Nueva actualización en tu caso", mensaje);

        if (prefs.RecibirPorWhatsApp && prefs.WhatsAppConfirmado)
            await _whatsapp.Enviar(caso.Cliente.Telefono, mensaje);
    }
}