[ApiController]
[Route("api/pruebas")]
[Authorize]
public class PruebasController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public PruebasController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // POST api/pruebas/subir
    [HttpPost("subir")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SubirPrueba(
        [FromForm] int casoId,
        [FromForm] string descripcion,
        [FromForm] IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("No se recibió ningún archivo.");

        var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".txt", ".docx" };
        var extension = Path.GetExtension(archivo.FileName).ToLower();

        if (!extensionesPermitidas.Contains(extension))
            return BadRequest("Tipo de archivo no permitido.");

        if (archivo.Length > 10 * 1024 * 1024)
            return BadRequest("El archivo no puede superar los 10MB.");

        var carpeta = Path.Combine(_env.WebRootPath, "uploads", "casos", casoId.ToString(), "pruebas");
        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await archivo.CopyToAsync(stream);
        }

        var url = $"/uploads/casos/{casoId}/pruebas/{nombreArchivo}";
        var prueba = new Prueba
        {
            Descripcion = descripcion,
            UrlArchivo  = url,
            Tipo        = extension.Replace(".", "").ToUpper(),
            CasoId      = casoId
        };

        _db.Pruebas.Add(prueba);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            prueba.Id,
            prueba.Descripcion,
            prueba.UrlArchivo,
            prueba.Tipo
        });
    }

    // GET api/pruebas/caso/{casoId}
    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetPruebasDeCaso(int casoId)
    {
        var pruebas = await _db.Pruebas
            .Where(p => p.CasoId == casoId)
            .ToListAsync();

        return Ok(pruebas);
    }

    // DELETE api/pruebas/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarPrueba(int id)
    {
        var prueba = await _db.Pruebas.FindAsync(id);
        if (prueba == null)
            return NotFound("Prueba no encontrada.");

        var rutaCompleta = Path.Combine(_env.WebRootPath, prueba.UrlArchivo.TrimStart('/'));
        if (System.IO.File.Exists(rutaCompleta))
            System.IO.File.Delete(rutaCompleta);

        _db.Pruebas.Remove(prueba);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Prueba eliminada correctamente." });
    }
}