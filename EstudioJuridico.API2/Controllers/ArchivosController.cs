using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/archivos")]
[Authorize]
public class ArchivosController : BaseController
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ArchivosController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

[HttpPost("subir")]
[Authorize(Roles = "Admin,Abogado,SuperAdmin")]
public async Task<IActionResult> SubirArchivo(
    [FromForm] int casoId,
    [FromForm] string categoria,
    [FromForm] int? seccionId,
    [FromForm] IFormFile archivo)
{
    if (archivo == null || archivo.Length == 0)
        return Error("No se recibió ningún archivo.");

    var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".txt", ".docx" };
    var extension = Path.GetExtension(archivo.FileName).ToLower();

    if (!extensionesPermitidas.Contains(extension))
        return Error("Tipo de archivo no permitido.");

    if (archivo.Length > 10 * 1024 * 1024)
        return Error("El archivo no puede superar los 10MB.");

    var carpeta = Path.Combine(_env.WebRootPath, "uploads", "casos", casoId.ToString());
    if (!Directory.Exists(carpeta))
        Directory.CreateDirectory(carpeta);

    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
    var rutaCompleta  = Path.Combine(carpeta, nombreArchivo);

    using var stream = new FileStream(rutaCompleta, FileMode.Create);
    await archivo.CopyToAsync(stream);

    var nuevoArchivo = new Archivo
    {
        Nombre                = archivo.FileName,
        Tipo                  = extension.Replace(".", "").ToUpper(),
        Categoria             = categoria,
        Url                   = $"/uploads/casos/{casoId}/{nombreArchivo}",
        CasoId                = casoId,
        SeccionExpedienteId   = seccionId
    };

    _db.Archivos.Add(nuevoArchivo);
    await _db.SaveChangesAsync();

    return Exito(new
    {
        nuevoArchivo.Id,
        nuevoArchivo.Nombre,
        nuevoArchivo.Tipo,
        nuevoArchivo.Url
    }, "Archivo subido correctamente.");
}

    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetArchivosDeCaso(int casoId)
    {
        var archivos = await _db.Archivos
            .Where(a => a.CasoId == casoId)
            .ToListAsync();

        return Exito(archivos);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> EliminarArchivo(int id)
    {
        var archivo = await _db.Archivos.FindAsync(id);
        if (archivo == null)
            return NoEncontrado("Archivo no encontrado.");

        var rutaCompleta = Path.Combine(_env.WebRootPath, archivo.Url.TrimStart('/'));
        if (System.IO.File.Exists(rutaCompleta))
            System.IO.File.Delete(rutaCompleta);

        _db.Archivos.Remove(archivo);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Archivo eliminado correctamente.");
    }
}