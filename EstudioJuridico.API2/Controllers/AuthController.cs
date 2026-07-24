using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Services.Interfaces;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseController
{
    
private readonly IAuthService _authService;
private readonly AppDbContext _db;
private readonly IConfiguration _configuration;

public AuthController(IAuthService authService, AppDbContext db, IConfiguration configuration)
{
    _authService   = authService;
    _db            = db;
    _configuration = configuration;
}


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var usuario = await _authService.Registrar(dto);
        if (usuario == null)
            return Error("El email ya está registrado.");

        return Exito(mensaje: "Cuenta creada correctamente.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var token = await _authService.Login(dto);
        if (token == null)
            return Error("Credenciales incorrectas.", 401);

        return Exito(new { token });
    }

    [HttpPost("crear-admin")]
    public async Task<IActionResult> CrearAdmin(RegisterDTO dto)
    {
        var usuario = await _authService.RegistrarAdmin(dto);
        if (usuario == null)
            return Error("El email ya está registrado.");

        return Exito(mensaje: "SuperAdmin creado correctamente.");
    }

    [HttpPost("crear-abogado")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> CrearAbogado(RegisterDTO dto)
    {
        var usuario = await _authService.RegistrarAbogado(dto);
        if (usuario == null)
            return Error("El email ya está registrado.");

        return Exito(mensaje: "Abogado creado correctamente.");
    }

[HttpPost("mantenimiento/reset-password")]
[AllowAnonymous]
public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
{
    var claveSecreta = Environment.GetEnvironmentVariable("MAINTENANCE_KEY")
        ?? _configuration["Mantenimiento:Clave"];

    if (string.IsNullOrEmpty(claveSecreta) || dto.ClaveMantenimiento != claveSecreta)
        return Error("No autorizado.", 403);

    var usuario = await _db.Usuarios
        .FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (usuario == null)
        return NoEncontrado("Usuario no encontrado.");

    usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);
    await _db.SaveChangesAsync();

    return Exito(mensaje: "Contraseña actualizada correctamente.");
}
}