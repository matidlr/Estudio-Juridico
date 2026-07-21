using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Services.Interfaces;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseController
{
    

private readonly IAuthService _authService;

public AuthController(IAuthService authService)
{
    _authService = authService;
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
}