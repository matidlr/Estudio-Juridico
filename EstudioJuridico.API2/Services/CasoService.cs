using EstudioJuridico.API2.Services;
using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Events;
using EstudioJuridico.API2.Repositories.Interfaces;
using EstudioJuridico.API2.Services.Interfaces;

public class CasoService : BaseService, ICasoService
{
    private readonly ICasoRepository _casoRepo;
    private readonly NotificacionManager _notificacionManager;

    public CasoService(ICasoRepository casoRepo, NotificacionManager notificacionManager)
    {
        _casoRepo            = casoRepo;
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

        var actualizacion = new Actualizacion
        {
            Contenido         = dto.Contenido,
            CasoId            = dto.CasoId,
            AutorId           = autorId,
            NroFoja           = dto.NroFoja,
            AclaracionCliente = dto.AclaracionCliente
        };

        // Verificar foja duplicada
        // Esta validación la dejamos en el controller por ahora

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