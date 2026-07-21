using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : BaseService, IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db     = db;
        _config = config;
    }

    public async Task<Usuario?> Registrar(RegisterDTO dto)
    {
        ValidarRequerido(dto.Email, "Email");
        ValidarRequerido(dto.Password, "Contraseña");
        ValidarRequerido(dto.Nombre, "Nombre");

        if (await _db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return null;

        var usuario = new Usuario
        {
            Nombre       = dto.Nombre,
            Apellido     = dto.Apellido,
            Email        = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol          = "Cliente"
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        var cliente = new Cliente
        {
            UsuarioId = usuario.Id,
            Telefono  = dto.Telefono,
            Preferencias = new PreferenciasNotificacion
            {
                RecibirPorEmail = true
            }
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return usuario;
    }

    public async Task<string?> Login(LoginDTO dto)
    {
        ValidarRequerido(dto.Email, "Email");
        ValidarRequerido(dto.Password, "Contraseña");

        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return null;

        return GenerarToken(usuario);
    }

    public string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email,          usuario.Email),
            new Claim(ClaimTypes.Role,           usuario.Rol),
            new Claim(ClaimTypes.Name,           $"{usuario.Nombre} {usuario.Apellido}")
        };

        var token = new JwtSecurityToken(
            issuer:            _config["Jwt:Issuer"],
            audience:          _config["Jwt:Audience"],
            claims:            claims,
            expires:           DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Usuario?> RegistrarAdmin(RegisterDTO dto)
    {
        ValidarRequerido(dto.Email, "Email");

        if (await _db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return null;

        var usuario = new Usuario
        {
            Nombre       = dto.Nombre,
            Apellido     = dto.Apellido,
            Email        = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol          = "SuperAdmin"
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        _db.Abogados.Add(new Abogado
        {
            UsuarioId    = usuario.Id,
            Matricula    = dto.Matricula    ?? "000000",
            Especialidad = dto.Especialidad ?? "General"
        });

        await _db.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario?> RegistrarAbogado(RegisterDTO dto)
    {
        ValidarRequerido(dto.Email, "Email");

        if (await _db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return null;

        var usuario = new Usuario
        {
            Nombre       = dto.Nombre,
            Apellido     = dto.Apellido,
            Email        = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol          = "Abogado"
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        _db.Abogados.Add(new Abogado
        {
            UsuarioId    = usuario.Id,
            Matricula    = dto.Matricula    ?? "000000",
            Especialidad = dto.Especialidad ?? "General"
        });

        await _db.SaveChangesAsync();
        return usuario;
    }
}