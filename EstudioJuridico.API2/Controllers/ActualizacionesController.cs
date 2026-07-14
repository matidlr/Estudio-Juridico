[ApiController]
[Route("api/actualizaciones")]
[Authorize]
public class ActualizacionesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ActualizacionesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("ultimas")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetUltimas()
    {
        var ultimas = await _db.Actualizaciones
            .Include(a => a.Caso)
            .Include(a => a.Autor)
            .OrderByDescending(a => a.Fecha)
            .Take(20)
            .Select(a => new
            {
                a.Id,
                a.Contenido,
                a.Fecha,
                a.NroFoja,
                Caratula = a.Caso.Caratula,
                CasoId   = a.Caso.Id,
                Autor    = a.Autor.Nombre + " " + a.Autor.Apellido
            })
            .ToListAsync();

        return Ok(ultimas);
    }
}