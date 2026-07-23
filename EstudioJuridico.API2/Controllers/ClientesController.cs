using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Repositories.Interfaces;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController : BaseController
{
    private readonly IClienteRepository _clienteRepo;

    public ClientesController(IClienteRepository clienteRepo)
    {
        _clienteRepo = clienteRepo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetTodos()
    {
        var clientes = await _clienteRepo.GetTodosAsync();

        var resultado = clientes.Select(c => new
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
        }).ToList();

        return Exito(resultado);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> GetPorId(int id)
    {
        var cliente = await _clienteRepo.GetByIdConCasosAsync(id);

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
        var cliente = await _clienteRepo.GetByUsuarioIdAsync(GetUsuarioId());

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
        var cliente = await _clienteRepo.GetByUsuarioIdAsync(GetUsuarioId());

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        cliente.Telefono         = dto.Telefono;
        cliente.Direccion        = dto.Direccion;
        cliente.Dni              = dto.Dni;
        cliente.Usuario.Nombre   = dto.Nombre;
        cliente.Usuario.Apellido = dto.Apellido;

        await _clienteRepo.UpdateAsync(cliente);

        return Exito(mensaje: "Perfil actualizado correctamente.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Abogado,SuperAdmin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var cliente = await _clienteRepo.GetByIdConCasosAsync(id);

        if (cliente == null)
            return NoEncontrado("Cliente no encontrado.");

        await _clienteRepo.DeleteAsync(id);

        return Exito(mensaje: "Cliente eliminado correctamente.");
    }
}