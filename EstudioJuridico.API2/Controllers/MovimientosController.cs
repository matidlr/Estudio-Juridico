[ApiController]
[Route("api/movimientos")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin,Cliente")]
public class MovimientosController : ControllerBase
{
    private readonly AppDbContext _db;

    public MovimientosController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/movimientos/caso/{casoId}
    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetMovimientosDeCaso(int casoId)
    {
        var movimientos = await _db.Movimientos
            .Where(m => m.CasoId == casoId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();

        var totalHonorarios = movimientos
            .Where(m => m.Tipo == "Honorario")
            .Sum(m => m.Monto);

        var totalGastos = movimientos
            .Where(m => m.Tipo == "Gasto")
            .Sum(m => m.Monto);

        var totalPagos = movimientos
            .Where(m => m.Tipo == "Pago")
            .Sum(m => m.Monto);

        return Ok(new
        {
            movimientos,
            resumen = new
            {
                totalHonorarios,
                totalGastos,
                totalPagos,
                saldoPendiente = totalHonorarios + totalGastos - totalPagos
            }
        });
    }

    // POST api/movimientos
    [HttpPost]
    public async Task<IActionResult> Crear(MovimientoDTO dto)
    {
        var movimiento = new MovimientoEconomico
        {
            Tipo      = dto.Tipo,
            Concepto  = dto.Concepto,
            Monto     = dto.Monto,
            Fecha     = dto.Fecha,
            FormaPago = dto.FormaPago,
            Notas     = dto.Notas,
            CasoId    = dto.CasoId
        };

        _db.Movimientos.Add(movimiento);
        await _db.SaveChangesAsync();

        return Ok(movimiento);
    }

    // DELETE api/movimientos/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var movimiento = await _db.Movimientos.FindAsync(id);
        if (movimiento == null)
            return NotFound("Movimiento no encontrado.");

        _db.Movimientos.Remove(movimiento);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Movimiento eliminado correctamente." });
    }

    // GET api/movimientos/mios
[HttpGet("mios")]
[Authorize(Roles = "Cliente,Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> GetMios()
{
    var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    var cliente = await _db.Clientes
        .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

    if (cliente == null)
        return NotFound("No se encontró el perfil de cliente.");

    var movimientos = await _db.Movimientos
        .Include(m => m.Caso)
        .Where(m => m.Caso.ClienteId == cliente.Id)
        .OrderByDescending(m => m.Fecha)
        .Select(m => new
        {
            m.Id,
            m.Tipo,
            m.Concepto,
            m.Monto,
            m.Fecha,
            m.FormaPago,
            Caratula = m.Caso.Caratula,
            CasoId   = m.Caso.Id
        })
        .ToListAsync();

    var totalHonorarios = movimientos.Where(m => m.Tipo == "Honorario").Sum(m => m.Monto);
    var totalGastos     = movimientos.Where(m => m.Tipo == "Gasto").Sum(m => m.Monto);
    var totalPagos      = movimientos.Where(m => m.Tipo == "Pago").Sum(m => m.Monto);

    return Ok(new
    {
        movimientos,
        resumen = new
        {
            totalHonorarios,
            totalGastos,
            totalPagos,
            saldoPendiente = totalHonorarios + totalGastos - totalPagos
        }
    });
}
}