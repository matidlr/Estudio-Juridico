using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Services.Interfaces;

[ApiController]
[Route("api/casos")]
[Authorize]
public class CasosController : BaseController
{
  private readonly ICasoService _casoService;
private readonly AppDbContext _db;
private readonly IWebHostEnvironment _env;

public CasosController(ICasoService casoService, AppDbContext db, IWebHostEnvironment env)
{
    _casoService = casoService;
    _db = db;
    _env = env;
}

    [HttpGet("mios")]
    public async Task<IActionResult> GetMisCasos()
    {
        var cliente = await _db.Clientes
            .FirstOrDefaultAsync(c => c.UsuarioId == GetUsuarioId());

        if (cliente == null)
            return NoEncontrado("No se encontró el perfil de cliente.");

        var casos = await _db.Casos
            .Include(c => c.Actualizaciones)
            .Include(c => c.Archivos)
            .Include(c => c.Pruebas)
            .Include(c => c.Comentarios)
            .Where(c => c.ClienteId == cliente.Id)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();

        return Exito(casos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCasoPorId(int id)
    {
        var caso = await _db.Casos
            .Include(c => c.Actualizaciones)
                .ThenInclude(a => a.Archivos)
            .Include(c => c.Archivos)
            .Include(c => c.Pruebas)
            .Include(c => c.Comentarios)
                .ThenInclude(com => com.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (caso == null)
            return NoEncontrado("Caso no encontrado.");

        return Exito(caso);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetTodosCasos()
    {
        var casos = await _db.Casos
            .Include(c => c.Cliente)
                .ThenInclude(cl => cl.Usuario)
            .Include(c => c.Abogado)
                .ThenInclude(a => a.Usuario)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();

        var resultado = casos.Select(c => new
        {
            c.Id,
            c.Caratula,
            c.Proceso,
            c.Juzgado,
            c.NroExpediente,
            c.Tipo,
            c.Estado,
            c.Etapa,
            c.FechaInicio,
            c.AbogadoId,
            c.ClienteId,
            Cliente = c.Cliente == null ? null : new
            {
                c.Cliente.Id,
                Nombre   = c.Cliente.Usuario?.Nombre,
                Apellido = c.Cliente.Usuario?.Apellido,
                Email    = c.Cliente.Usuario?.Email
            },
            Abogado = c.Abogado == null ? null : new
            {
                c.Abogado.Id,
                Nombre   = c.Abogado.Usuario?.Nombre,
                Apellido = c.Abogado.Usuario?.Apellido
            }
        }).ToList();

        return Exito(resultado);
    }

    [HttpPost]
    [Authorize(Roles = "Abogado,SuperAdmin")]
    public async Task<IActionResult> CrearCaso(CasoDTO dto)
    {
        var abogado = await _db.Abogados
            .FirstOrDefaultAsync(a => a.UsuarioId == GetUsuarioId());

        if (abogado == null)
            return NoEncontrado("No se encontró el perfil de abogado.");

        var caso = await _casoService.CrearCaso(dto, abogado.Id);
        return Exito(caso, "Caso creado correctamente.");
    }

    [HttpPost("actualizacion")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> AgregarActualizacion(ActualizacionDTO dto)
    {
        await _casoService.AgregarActualizacion(dto, GetUsuarioId());
        return Exito(mensaje: "Actualización guardada. Cliente notificado.");
    }

    [HttpPost("comentario")]
    public async Task<IActionResult> AgregarComentario(ComentarioDTO dto)
    {
        var comentario = new Comentario
        {
            Texto            = dto.Texto,
            CasoId           = dto.CasoId,
            UsuarioId        = GetUsuarioId(),
            VisibleAlAbogado = dto.VisibleAlAbogado,
            Fecha            = DateTime.UtcNow,
            TipoAutor        = EsCliente() ? "Cliente" : "Abogado"
        };

        _db.Comentarios.Add(comentario);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Comentario agregado correctamente.");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> EditarCaso(int id, CasoDTO dto)
    {
        var caso = await _db.Casos.FindAsync(id);
        if (caso == null)
            return NoEncontrado("Caso no encontrado.");

        caso.Caratula      = dto.Caratula;
        caso.Proceso       = dto.Proceso;
        caso.Juzgado       = dto.Juzgado;
        caso.NroExpediente = dto.NroExpediente;
        caso.Tipo          = dto.Tipo;
        caso.Estado        = dto.Estado;
        caso.Etapa         = dto.Etapa;

        await _db.SaveChangesAsync();
        return Exito(caso, "Caso actualizado correctamente.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> EliminarCaso(int id)
    {
        var caso = await _db.Casos.FindAsync(id);
        if (caso == null)
            return NoEncontrado("Caso no encontrado.");

        _db.Casos.Remove(caso);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Caso eliminado correctamente.");
    }

    [HttpPut("{id}/reasignar")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> ReasignarAbogado(int id, [FromBody] ReasignarAbogadoDTO dto)
    {
        var caso = await _db.Casos.FindAsync(id);
        if (caso == null)
            return NoEncontrado("Caso no encontrado.");

        var abogado = await _db.Abogados.FindAsync(dto.AbogadoId);
        if (abogado == null)
            return NoEncontrado("Abogado no encontrado.");

        caso.AbogadoId = dto.AbogadoId;
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Abogado reasignado correctamente.");
    }

    [HttpPost("actualizacion-con-archivo")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> AgregarActualizacionConArchivo(
        [FromForm] string contenido,
        [FromForm] int casoId,
        [FromForm] string? nroFoja,
        [FromForm] IFormFile? archivo)
    {
        var actualizacion = new Actualizacion
        {
            Contenido = contenido,
            CasoId    = casoId,
            AutorId   = GetUsuarioId(),
            NroFoja   = nroFoja
        };

        _db.Actualizaciones.Add(actualizacion);
        await _db.SaveChangesAsync();

        if (archivo != null && archivo.Length > 0)
        {
            var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".txt", ".docx" };
            var extension = Path.GetExtension(archivo.FileName).ToLower();

            if (extensionesPermitidas.Contains(extension))
            {
                var carpeta = Path.Combine(_env.WebRootPath, "uploads", "casos", casoId.ToString(), "fojas");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta  = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                await archivo.CopyToAsync(stream);

                _db.Archivos.Add(new Archivo
                {
                    Nombre          = archivo.FileName,
                    Tipo            = extension.Replace(".", "").ToUpper(),
                    Categoria       = "Foja",
                    Url             = $"/uploads/casos/{casoId}/fojas/{nombreArchivo}",
                    CasoId          = casoId,
                    ActualizacionId = actualizacion.Id
                });

                await _db.SaveChangesAsync();
            }
        }

        await _casoService.NotificarCliente(casoId);
        return Exito(mensaje: "Foja agregada correctamente.");
    }

    [HttpGet("admin/consultas-pendientes")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetConsultasPendientes()
    {
        var consultas = await _db.Comentarios
            .Include(c => c.Caso)
            .Include(c => c.Usuario)
            .Where(c => c.TipoAutor == "Cliente")
            .OrderByDescending(c => c.Fecha)
            .Select(c => new
            {
                c.Id,
                c.Texto,
                c.Fecha,
                c.TipoAutor,
                c.Leida,
                CasoId        = c.Caso.Id,
                Caratula      = c.Caso.Caratula,
                Cliente       = c.Usuario.Nombre + " " + c.Usuario.Apellido,
                TieneRespuesta = _db.Comentarios.Any(r =>
                    r.CasoId == c.CasoId &&
                    r.TipoAutor == "Abogado" &&
                    r.Fecha > c.Fecha)
            })
            .ToListAsync();

        return Exito(consultas);
    }

    [HttpDelete("comentario/{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> EliminarComentario(int id)
    {
        var comentario = await _db.Comentarios.FindAsync(id);
        if (comentario == null)
            return NoEncontrado("Comentario no encontrado.");

        _db.Comentarios.Remove(comentario);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Comentario eliminado correctamente.");
    }

    [HttpPut("comentario/{id}/leida")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> MarcarComentarioLeido(int id)
    {
        var comentario = await _db.Comentarios.FindAsync(id);
        if (comentario == null)
            return NoEncontrado("Comentario no encontrado.");

        comentario.Leida = !comentario.Leida;
        await _db.SaveChangesAsync();

        return Exito(new { leida = comentario.Leida },
            comentario.Leida ? "Consulta marcada como leída." : "Consulta marcada como no leída.");
    }

    [HttpGet("estadisticas")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetEstadisticas([FromQuery] int meses = 1)
    {
        var desde = DateTime.UtcNow.AddMonths(-meses);

        var totalCasos      = await _db.Casos.CountAsync();
        var casosActivos    = await _db.Casos.CountAsync(c => c.Estado == "Activo");
        var casosFinalizados = await _db.Casos.CountAsync(c => c.Estado == "Finalizado");
        var casosNuevos     = await _db.Casos.CountAsync(c => c.FechaInicio >= desde);

        var casosPorTipo = await _db.Casos
            .GroupBy(c => c.Tipo)
            .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
            .ToListAsync();

        var casosPorAbogado = await _db.Casos
            .Include(c => c.Abogado).ThenInclude(a => a.Usuario)
            .GroupBy(c => new { c.AbogadoId, c.Abogado.Usuario.Nombre, c.Abogado.Usuario.Apellido })
            .Select(g => new
            {
                Abogado  = g.Key.Nombre + " " + g.Key.Apellido,
                Cantidad = g.Count(),
                Activos  = g.Count(c => c.Estado == "Activo")
            })
            .ToListAsync();

        var casosPorEstado = await _db.Casos
            .GroupBy(c => c.Estado)
            .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
            .ToListAsync();

        var fojasCargadas        = await _db.Actualizaciones.CountAsync(a => a.Fecha >= desde);
        var consultasRespondidas = await _db.Comentarios.CountAsync(c => c.TipoAutor == "Abogado" && c.Fecha >= desde);
        var vencimientosProximos = await _db.Recordatorios.CountAsync(r =>
            !r.Enviado && r.FechaEnvio >= DateTime.UtcNow && r.FechaEnvio <= DateTime.UtcNow.AddDays(30));

        var totalHonorarios = await _db.Movimientos.Where(m => m.Tipo == "Honorario").SumAsync(m => m.Monto);
        var totalPagos      = await _db.Movimientos.Where(m => m.Tipo == "Pago").SumAsync(m => m.Monto);
        var totalGastos     = await _db.Movimientos.Where(m => m.Tipo == "Gasto").SumAsync(m => m.Monto);

        var causasPorMes = await _db.Casos
            .Where(c => c.FechaInicio >= desde)
            .GroupBy(c => new { c.FechaInicio.Year, c.FechaInicio.Month })
            .Select(g => new { Año = g.Key.Year, Mes = g.Key.Month, Cantidad = g.Count() })
            .OrderBy(g => g.Año).ThenBy(g => g.Mes)
            .ToListAsync();

        return Exito(new
        {
            causas = new { total = totalCasos, activas = casosActivos, finalizadas = casosFinalizados, nuevas = casosNuevos },
            casosPorTipo,
            casosPorAbogado,
            casosPorEstado,
            actividad = new { fojasCargadas, consultasRespondidas, vencimientosProximos },
            economico = new { totalHonorarios, totalGastos, totalPagos, saldoPendiente = totalHonorarios + totalGastos - totalPagos },
            causasPorMes
        });
    }

[HttpGet("{id}/fojas")]
public async Task<IActionResult> GetFojasPaginadas(
    int id,
    [FromQuery] int pagina = 1,
    [FromQuery] int? seccionId = null,
    [FromQuery] string? busqueda = null)
{
    var query = _db.Actualizaciones
        .Where(a => a.CasoId == id);

    if (seccionId.HasValue)
        query = query.Where(a => a.SeccionExpedienteId == seccionId);

    if (!string.IsNullOrEmpty(busqueda))
        query = query.Where(a =>
            a.NroFoja!.Contains(busqueda) ||
            a.Contenido.Contains(busqueda));

    var total = await query.CountAsync();

    var foja = await query
        .OrderBy(a => a.NroFoja)
        .Skip(pagina - 1)
        .Take(1)
        .Select(a => new
        {
            a.Id,
            a.Contenido,
            a.Fecha,
            a.NroFoja,
            a.AclaracionCliente,
            a.SeccionExpedienteId
        })
        .FirstOrDefaultAsync();

    return Exito(new
    {
        foja,
        total,
        pagina,
        totalPaginas = total
    });
}

[HttpPut("actualizacion/{id}")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> EditarActualizacion(int id, ActualizacionDTO dto)
{
    var actualizacion = await _db.Actualizaciones.FindAsync(id);
    if (actualizacion == null)
        return NoEncontrado("Foja no encontrada.");

    // Verificamos que no exista otra foja con el mismo número
    if (!string.IsNullOrEmpty(dto.NroFoja))
    {
        var fojaExiste = await _db.Actualizaciones
            .AnyAsync(a => a.CasoId == actualizacion.CasoId && 
                          a.NroFoja == dto.NroFoja && 
                          a.Id != id);
        if (fojaExiste)
            throw new ArgumentException($"Ya existe una foja con el número {dto.NroFoja} en este expediente.");
    }

    actualizacion.Contenido         = dto.Contenido;
    actualizacion.NroFoja           = dto.NroFoja;
    actualizacion.AclaracionCliente = dto.AclaracionCliente;

    await _db.SaveChangesAsync();
    return Exito(mensaje: "Foja actualizada correctamente.");
}

[HttpDelete("actualizacion/{id}")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> EliminarActualizacion(int id)
{
    var actualizacion = await _db.Actualizaciones.FindAsync(id);
    if (actualizacion == null)
        return NoEncontrado("Foja no encontrada.");

    _db.Actualizaciones.Remove(actualizacion);
    await _db.SaveChangesAsync();

    return Exito(mensaje: "Foja eliminada correctamente.");
}
}