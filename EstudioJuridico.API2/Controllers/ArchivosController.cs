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
    [FromForm] int? actualizacionId,
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
        Nombre              = archivo.FileName,
        Tipo                = extension.Replace(".", "").ToUpper(),
        Categoria           = categoria,
        Url                 = $"/uploads/casos/{casoId}/{nombreArchivo}",
        CasoId              = casoId,
        SeccionExpedienteId = seccionId,
        ActualizacionId     = actualizacionId
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

    [HttpGet("foja/{actualizacionId}")]
[Authorize]
public async Task<IActionResult> GetArchivosDeFoja(int actualizacionId)
{
    var archivos = await _db.Archivos
        .Where(a => a.ActualizacionId == actualizacionId)
        .Select(a => new { a.Id, a.Nombre, a.Tipo, a.Url, a.Categoria })
        .ToListAsync();

    return Exito(archivos);
}

[HttpGet("descargar/{id}")]
[Authorize]
public async Task<IActionResult> Descargar(int id)
{
    var archivo = await _db.Archivos.FindAsync(id);
    if (archivo == null)
        return NoEncontrado("Archivo no encontrado.");

    // Si es cliente verificamos que el archivo pertenezca a su caso
    if (GetRol() == "Cliente")
    {
        var usuarioId = GetUsuarioId();
        var cliente = await _db.Clientes
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        var caso = await _db.Casos
            .FirstOrDefaultAsync(c => c.Id == archivo.CasoId && c.ClienteId == cliente!.Id);

        if (caso == null)
            return Error("No tenés permiso para acceder a este archivo.", 403);
    }

    var rutaCompleta = Path.Combine(_env.WebRootPath, archivo.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

    if (!System.IO.File.Exists(rutaCompleta))
        return NoEncontrado("Archivo no encontrado en el servidor.");

    var tipoContenido = archivo.Tipo switch
    {
        "PDF"  => "application/pdf",
        "JPG"  => "image/jpeg",
        "JPEG" => "image/jpeg",
        "PNG"  => "image/png",
        "TXT"  => "text/plain",
        "DOCX" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        _      => "application/octet-stream"
    };

    var bytes = await System.IO.File.ReadAllBytesAsync(rutaCompleta);
    return File(bytes, tipoContenido, archivo.Nombre);
}
}