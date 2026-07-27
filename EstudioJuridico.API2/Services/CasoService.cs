using EstudioJuridico.API2.Services;
using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Events;
using EstudioJuridico.API2.Repositories.Interfaces;
using EstudioJuridico.API2.Services.Interfaces;

public class CasoService : BaseService, ICasoService
{
    private readonly ICasoRepository _casoRepo;
    private readonly NotificacionManager _notificacionManager;
    private readonly IActualizacionRepository _actualizacionRepo;

   public CasoService(ICasoRepository casoRepo, IActualizacionRepository actualizacionRepo, NotificacionManager notificacionManager)
{
    _casoRepo            = casoRepo;
    _actualizacionRepo   = actualizacionRepo;
    _notificacionManager = notificacionManager;
}

    public async Task<List<Caso>> GetCasosDeCliente(int clienteId)
    {
        return await _casoRepo.GetPorClienteAsync(clienteId);
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

        return await _casoRepo.CreateAsync(caso);
    }

 public async Task AgregarActualizacion(ActualizacionDTO dto, int autorId)
{
    ValidarRequerido(dto.Contenido, "Contenido");

    var caso = await _casoRepo.GetByIdAsync(dto.CasoId);
    if (caso == null) throw new KeyNotFoundException("Caso no encontrado.");

    // Validar número de foja único
    if (!string.IsNullOrEmpty(dto.NroFoja))
    {
        var existe = await _actualizacionRepo.ExisteFojaAsync(dto.CasoId, dto.NroFoja);
        if (existe)
            throw new InvalidOperationException($"Ya existe una foja con el número '{dto.NroFoja}' en esta causa.");
    }

    // Validar largo del contenido
    if (dto.Contenido.Length > 50000)
        throw new InvalidOperationException("El contenido no puede superar los 50.000 caracteres.");

    var actualizacion = new Actualizacion
    {
        Contenido         = dto.Contenido,
        CasoId            = dto.CasoId,
        AutorId           = autorId,
        NroFoja           = dto.NroFoja,
        AclaracionCliente = dto.AclaracionCliente
    };

    await _actualizacionRepo.CreateAsync(actualizacion);

    await _notificacionManager.NotificarFojaAgregada(new FojaAgregadaEvent
    {
        CasoId   = dto.CasoId,
        Caratula = caso.Caratula,
        NroFoja  = dto.NroFoja,
        Fecha    = DateTime.UtcNow
    });
}

    public async Task NotificarCliente(int casoId)
    {
        var caso = await _casoRepo.GetByIdAsync(casoId);

        await _notificacionManager.NotificarFojaAgregada(new FojaAgregadaEvent
        {
            CasoId   = casoId,
            Caratula = caso?.Caratula ?? "",
            Fecha    = DateTime.UtcNow
        });
    }

    
}