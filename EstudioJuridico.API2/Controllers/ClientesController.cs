// Controllers/ClientesController.cs
[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClientesController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/clientes
    // Solo el abogado (Admin) puede ver todos los clientes
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
                Nombre   = c.Usuario.Nombre,
                Apellido = c.Usuario.Apellido,
                Email    = c.Usuario.Email,
                Notificaciones = new
                {
                    c.Preferencias!.RecibirPorEmail,
                    c.Preferencias!.RecibirPorWhatsApp
                },
                CasosActivos = c.Casos.Count(caso => caso.Estado == "Activo")
            })
            .ToListAsync();

        return Ok(clientes);
    }

    // GET api/clientes/{id}
    // El abogado puede ver el detalle completo de un cliente específico
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
            return NotFound("Cliente no encontrado.");

        return Ok(new
        {
            cliente.Id,
            cliente.Dni,
            cliente.Telefono,
            cliente.Direccion,
            Nombre   = cliente.Usuario.Nombre,
            Apellido = cliente.Usuario.Apellido,
            Email    = cliente.Usuario.Email,
            Notificaciones = new
            {
                cliente.Preferencias!.RecibirPorEmail,
                cliente.Preferencias!.RecibirPorWhatsApp
            },
            Casos = cliente.Casos.Select(caso => new
            {
                caso.Id,
                caso.caratula,
                caso.Tipo,
                caso.Estado,
                caso.Etapa,
                caso.FechaInicio,
                caso.FechaCierre
            })
        });
    }

    // GET api/clientes/miperfil
    // El cliente ve y edita su propio perfil
    [HttpGet("miperfil")]
    public async Task<IActionResult> GetMiPerfil()
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .Include(c => c.Preferencias)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (cliente == null)
            return NotFound("Cliente no encontrado.");

        return Ok(new
        {
            cliente.Id,
            cliente.Dni,
            cliente.Telefono,
            cliente.Direccion,
            Nombre   = cliente.Usuario.Nombre,
            Apellido = cliente.Usuario.Apellido,
            Email    = cliente.Usuario.Email,
            Notificaciones = new
            {
                cliente.Preferencias!.RecibirPorEmail,
                cliente.Preferencias!.RecibirPorWhatsApp
            }
        });
    }

    // PUT api/clientes/miperfil
    // El cliente actualiza sus propios datos
    [HttpPut("miperfil")]
    public async Task<IActionResult> ActualizarMiPerfil(ActualizarPerfilDTO dto)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (cliente == null)
            return NotFound("Cliente no encontrado.");

        // Actualizamos los datos del perfil extendido
        cliente.Telefono  = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.Dni       = dto.Dni;

        // Actualizamos los datos del usuario base
        cliente.Usuario.Nombre   = dto.Nombre;
        cliente.Usuario.Apellido = dto.Apellido;

        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Perfil actualizado correctamente." });
    }

    // DELETE api/clientes/{id}
    // Solo el abogado puede eliminar un cliente
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound("Cliente no encontrado.");

        // Eliminamos el cliente y su usuario base
        _db.Clientes.Remove(cliente);
        _db.Usuarios.Remove(cliente.Usuario);

        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Cliente eliminado correctamente." });
    }
}