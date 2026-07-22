using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Events;
using EstudioJuridico.API2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EstudioJuridico.API2.Services
{
    public class CasoService : BaseService, ICasoService
    {
    private readonly AppDbContext _db;
    private readonly NotificacionManager _notificacionManager;

    public CasoService(AppDbContext db, NotificacionManager notificacionManager)
    {
        _db                  = db;
        _notificacionManager = notificacionManager;
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

    // Verificamos que no exista otra foja con el mismo número en el mismo caso
    if (!string.IsNullOrEmpty(dto.NroFoja))
    {
        var fojaExiste = await _db.Actualizaciones
            .AnyAsync(a => a.CasoId == dto.CasoId && a.NroFoja == dto.NroFoja);

        if (fojaExiste)
            throw new ArgumentException($"Ya existe una foja con el número {dto.NroFoja} en este expediente.");
    }

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

    var caso = await _db.Casos.FindAsync(dto.CasoId);

    await _notificacionManager.NotificarFojaAgregada(new FojaAgregadaEvent
    {
        CasoId   = dto.CasoId,
        Caratula = caso?.Caratula ?? "",
        NroFoja  = dto.NroFoja,
        Fecha    = DateTime.UtcNow
    });
}

    public async Task NotificarCliente(int casoId)
    {
        var caso = await _db.Casos.FindAsync(casoId);

        await _notificacionManager.NotificarFojaAgregada(new FojaAgregadaEvent
        {
            CasoId   = casoId,
            Caratula = caso?.Caratula ?? "",
            Fecha    = DateTime.UtcNow
        });
    }
}}