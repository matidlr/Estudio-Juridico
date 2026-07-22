using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/secciones")]
[Authorize]
public class SeccionesController : BaseController
{
    private readonly AppDbContext _db;

    public SeccionesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetSeccionesDeCaso(int casoId)
    {
        var secciones = await _db.Secciones
            .Include(s => s.Actualizaciones)
            .Where(s => s.CasoId == casoId)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.FojaDesde)
            .Select(s => new
            {
                s.Id,
                s.Titulo,
                s.Descripcion,
                s.FojaDesde,
                s.FojaHasta,
                s.Orden,
                s.CasoId,
                CantidadFojas = s.Actualizaciones.Count
            })
            .ToListAsync();

        return Exito(secciones);
    }

   [HttpPost]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> Crear(SeccionExpedienteDTO dto)
{
    var seccion = new SeccionExpediente
    {
        Titulo      = dto.Titulo,
        Descripcion = dto.Descripcion,
        FojaDesde   = dto.FojaDesde,
        FojaHasta   = dto.FojaHasta,
        Orden       = dto.Orden,
        CasoId      = dto.CasoId
    };

    _db.Secciones.Add(seccion);
    await _db.SaveChangesAsync();

    // Asignamos automáticamente las fojas existentes en el rango
    var fojas = await _db.Actualizaciones
        .Where(a => a.CasoId == dto.CasoId)
        .ToListAsync();

    foreach (var foja in fojas)
    {
        if (foja.NroFoja != null && 
            int.TryParse(foja.NroFoja, out int nro) &&
            nro >= dto.FojaDesde && 
            nro <= dto.FojaHasta)
        {
            foja.SeccionExpedienteId = seccion.Id;
        }
    }

    await _db.SaveChangesAsync();

    return Exito(seccion, "Sección creada correctamente.");
}

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> Editar(int id, SeccionExpedienteDTO dto)
    {
        var seccion = await _db.Secciones.FindAsync(id);
        if (seccion == null)
            return NoEncontrado("Sección no encontrada.");

        seccion.Titulo      = dto.Titulo;
        seccion.Descripcion = dto.Descripcion;
        seccion.FojaDesde   = dto.FojaDesde;
        seccion.FojaHasta   = dto.FojaHasta;
        seccion.Orden       = dto.Orden;

        await _db.SaveChangesAsync();
        return Exito(mensaje: "Sección actualizada correctamente.");
    }

    [HttpDelete("{id}")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> Eliminar(int id)
{
    var seccion = await _db.Secciones.FindAsync(id);
    if (seccion == null)
        return NoEncontrado("Sección no encontrada.");

    // Desvinculamos las fojas de la sección sin borrarlas
    var fojas = await _db.Actualizaciones
        .Where(a => a.SeccionExpedienteId == id)
        .ToListAsync();

    foreach (var foja in fojas)
        foja.SeccionExpedienteId = null;

    _db.Secciones.Remove(seccion);
    await _db.SaveChangesAsync();

    return Exito(mensaje: "Sección eliminada correctamente.");
}
}