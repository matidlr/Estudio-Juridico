using EstudioJuridico.API2.Base;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController : BaseController
{
    private readonly AppDbContext _db;

    public ClientesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetTodos()
    {
        var clientes = await _db.Clientes
            .Include(c => c.Usuario)
            .Include(c => c.Preferencias)
            .Include(c => c.Casos)
            .Select(c => new
            {
                c.Id,
                c.Dni,
                c.Telefono,
                c.Direccion,
                Nombre         = c.Usuario.Nombre,
                Apellido       = c.Usuario.Apellido,
                Email          = c.Usuario.Email,
                Notificaciones = new
                {
                    c.Preferencias!.RecibirPorEmail,
                    c.Preferencias!.RecibirPorWhatsApp
                },
                CasosActivos = c.Casos.Count(caso => caso.Estado == "Activo")
            })
            .ToListAsync();

        return Exito(clientes);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .Include(c => c.Preferencias)
            .Include(c => c.Casos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        return Exito(new
        {
            cliente.Id,
            cliente.Dni,
            cliente.Telefono,
            cliente.Direccion,
            Nombre         = cliente.Usuario.Nombre,
            Apellido       = cliente.Usuario.Apellido,
            Email          = cliente.Usuario.Email,
            Notificaciones = new
            {
                cliente.Preferencias!.RecibirPorEmail,
                cliente.Preferencias!.RecibirPorWhatsApp
            },
            Casos = cliente.Casos.Select(caso => new
            {
                caso.Id,
                caso.Caratula,
                caso.Tipo,
                caso.Estado,
                caso.Etapa,
                caso.FechaInicio,
                caso.FechaCierre
            })
        });
    }

    [HttpGet("miperfil")]
    public async Task<IActionResult> GetMiPerfil()
    {
        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .Include(c => c.Preferencias)
            .FirstOrDefaultAsync(c => c.UsuarioId == GetUsuarioId());

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        return Exito(new
        {
            cliente.Id,
            cliente.Dni,
            cliente.Telefono,
            cliente.Direccion,
            Nombre         = cliente.Usuario.Nombre,
            Apellido       = cliente.Usuario.Apellido,
            Email          = cliente.Usuario.Email,
            Notificaciones = new
            {
                cliente.Preferencias!.RecibirPorEmail,
                cliente.Preferencias!.RecibirPorWhatsApp
            }
        });
    }

    [HttpPut("miperfil")]
    public async Task<IActionResult> ActualizarMiPerfil(ActualizarPerfilDTO dto)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.UsuarioId == GetUsuarioId());

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        cliente.Telefono         = dto.Telefono;
        cliente.Direccion        = dto.Direccion;
        cliente.Dni              = dto.Dni;
        cliente.Usuario.Nombre   = dto.Nombre;
        cliente.Usuario.Apellido = dto.Apellido;

        await _db.SaveChangesAsync();

        return Exito(mensaje: "Perfil actualizado correctamente.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        _db.Clientes.Remove(cliente);
        _db.Usuarios.Remove(cliente.Usuario);
        await _db.SaveChangesAsync();

        return Exito(mensaje: "Cliente eliminado correctamente.");
    }
}