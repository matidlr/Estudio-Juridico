using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
// Se encarga del registro, login y generación del JWT.
public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<Usuario?> Registrar(RegisterDTO dto)
    {
        // Verificamos que el email no exista
        if (await _db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return null;

        // Encriptamos la contraseña antes de guardarla
        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = "Cliente"
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        // Creamos el perfil de cliente vinculado al usuario
        var cliente = new Cliente
        {
            UsuarioId = usuario.Id,
            Telefono = dto.Telefono,
            Preferencias = new PreferenciasNotificacion
            {
                RecibirPorEmail = true  // por defecto activo el email
            }
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return usuario;
    }

    public async Task<string?> Login(LoginDTO dto)
    {
        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        // Verificamos contraseña contra el hash guardado
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return null;

        return GenerarToken(usuario);
    }

    private string GenerarToken(Usuario usuario)
    {
        // El JWT contiene el id y el rol del usuario.
        // El frontend lo guarda y lo manda en cada request.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}