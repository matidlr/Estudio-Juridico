// Controllers/CasosController.cs
[ApiController]
[Route("api/casos")]
[Authorize]   // ← solo usuarios con JWT válido pueden acceder
public class CasosController : ControllerBase
{
    private readonly CasoService _casoService;
    public CasosController(CasoService casoService) { _casoService = casoService; }

    // GET api/casos/mios  → el cliente ve solo sus propios casos
    [HttpGet("mios")]
    public async Task<IActionResult> GetMisCasos()
    {
        // Extraemos el id del cliente desde el JWT
        var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var casos = await _casoService.GetCasosDeCliente(clienteId);
        return Ok(casos);
    }

    // POST api/casos  → solo el abogado (Admin) puede crear casos
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CrearCaso(CasoDTO dto)
    {
        var abogadoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var caso = await _casoService.CrearCaso(dto, abogadoId);
        return CreatedAtAction(nameof(GetMisCasos), new { id = caso.Id }, caso);
    }

    // POST api/casos/actualizacion  → abogado agrega novedad y notifica al cliente
    [HttpPost("actualizacion")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AgregarActualizacion(ActualizacionDTO dto)
    {
        var autorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _casoService.AgregarActualizacion(dto, autorId);
        return Ok(new { mensaje = "Actualización guardada. Cliente notificado." });
    }
}