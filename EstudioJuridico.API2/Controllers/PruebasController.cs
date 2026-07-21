using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/pruebas")]
[Authorize]
public class PruebasController : BaseController
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public PruebasController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpPost("subir")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> SubirPrueba(
        [FromForm] int casoId,
        [FromForm] string descripcion,
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

        var carpeta = Path.Combine(_env.WebRootPath, "uploads", "casos", casoId.ToString(), "pruebas");
        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta  = Path.Combine(carpeta, nombreArchivo);

        using var stream = new FileStream(rutaCompleta, FileMode.Create);
        await archivo.CopyToAsync(stream);

        var prueba = new Prueba
        {
            Descripcion = descripcion,
            UrlArchivo  = $"/uploads/casos/{casoId}/pruebas/{nombreArchivo}",
            Tipo        = extension.Replace(".", "").ToUpper(),
            CasoId      = casoId
        };

        _db.Pruebas.Add(prueba);
        await _db.SaveChangesAsync();

        return Exito(new
        {
            prueba.Id,
            prueba.Descripcion,
            prueba.UrlArchivo,
            prueba.Tipo
        }, "Prueba subida correctamente.");
    }

    [HttpGet("caso/{casoId}")]
    public async Task<IActionResult> GetPruebasDeCaso(int casoId)
    {
        var pruebas = await _db.Pruebas
            .Where(p => p.CasoId == casoId)
            .ToListAsync();

        return Exito(pruebas);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> EliminarPrueba(int id)
    {
        var prueba = await _db.Pruebas.FindAsync(id);
        if (prueba == null)
            return NoEncontrado("Prueba no encontrada.");

        var rutaCompleta = Path.Combine(_env.WebRootPath, prueba.UrlArchivo.TrimStart('/'));
        if (System.IO.File.Exists(rutaCompleta))
            System.IO.File.Delete(rutaCompleta);

        _db.Pruebas.Remove(prueba);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Prueba eliminada correctamente.");
    }
}