using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService) { _authService = authService; }

    // POST api/auth/register  → crea cuenta de cliente
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var usuario = await _authService.Registrar(dto);
        if (usuario == null)
            return BadRequest("El email ya está registrado.");
        return Ok(new { mensaje = "Cuenta creada correctamente." });
    }

    // POST api/auth/login  → devuelve el JWT si las credenciales son correctas
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var token = await _authService.Login(dto);
        if (token == null)
            return Unauthorized("Credenciales incorrectas.");
        return Ok(new { token });
    }

    // POST api/auth/crear-admin
// Endpoint temporal para crear el primer admin
// IMPORTANTE: eliminarlo después de crear el admin
[HttpPost("crear-admin")]
public async Task<IActionResult> CrearAdmin(RegisterDTO dto)
{
    var usuario = await _authService.RegistrarAdmin(dto);
    if (usuario == null)
        return BadRequest("El email ya está registrado.");
    return Ok(new { mensaje = "Admin creado correctamente." });
}

// POST api/auth/crear-abogado — solo SuperAdmin
[HttpPost("crear-abogado")]
[Authorize(Roles = "SuperAdmin")]
public async Task<IActionResult> CrearAbogado(RegisterDTO dto)
{
    var usuario = await _authService.RegistrarAbogado(dto);
    if (usuario == null)
        return BadRequest("El email ya está registrado.");
    return Ok(new { mensaje = "Abogado creado correctamente." });
}
}