[ApiController]
[Route("api/archivos")]
[Authorize]
public class ArchivosController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ArchivosController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // POST api/archivos/subir
    // Sube un archivo adjunto a un caso
    [HttpPost("subir")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SubirArchivo(
        [FromForm] int casoId,
        [FromForm] string categoria,
        [FromForm] IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("No se recibió ningún archivo.");

        // Validamos el tipo de archivo
        var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".txt", ".docx" };
        var extension = Path.GetExtension(archivo.FileName).ToLower();

        if (!extensionesPermitidas.Contains(extension))
            return BadRequest("Tipo de archivo no permitido.");

        // Limitamos el tamaño a 10MB
        if (archivo.Length > 10 * 1024 * 1024)
            return BadRequest("El archivo no puede superar los 10MB.");

        // Creamos la carpeta del caso si no existe
        var carpeta = Path.Combine(_env.WebRootPath, "uploads", "casos", casoId.ToString());
        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        // Generamos un nombre único para evitar colisiones
        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        // Guardamos el archivo en disco
        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await archivo.CopyToAsync(stream);
        }

        // Guardamos la referencia en la base de datos
        var url = $"/uploads/casos/{casoId}/{nombreArchivo}";
        var nuevoArchivo = new Archivo
        {
            Nombre    = archivo.FileName,
            Tipo      = extension.Replace(".", "").ToUpper(),
            Categoria = categoria,
            Url       = url,
            CasoId    = casoId
        };

        _db.Archivos.Add(nuevoArchivo);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            nuevoArchivo.Id,
            nuevoArchivo.Nombre,
            nuevoArchivo.Tipo,
            nuevoArchivo.Url
        });
    }

    // GET api/archivos/caso/{casoId}
    // Devuelve todos los archivos de un caso
    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetArchivosDeCaso(int casoId)
    {
        var archivos = await _db.Archivos
            .Where(a => a.CasoId == casoId)
            .ToListAsync();

        return Ok(archivos);
    }

    // DELETE api/archivos/{id}
    // Elimina un archivo
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarArchivo(int id)
    {
        var archivo = await _db.Archivos.FindAsync(id);
        if (archivo == null)
            return NotFound("Archivo no encontrado.");

        // Eliminamos el archivo del disco
        var rutaCompleta = Path.Combine(_env.WebRootPath, archivo.Url.TrimStart('/'));
        if (System.IO.File.Exists(rutaCompleta))
            System.IO.File.Delete(rutaCompleta);

        _db.Archivos.Remove(archivo);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Archivo eliminado correctamente." });
    }
}