// DTOs/LoginDTO.cs
public class LoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// DTOs/RegisterDTO.cs


// DTOs/CasoDTO.cs
// Lo que el frontend manda para crear o editar un caso
public class CasoDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string NombrePartes { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo";
    public string Etapa { get; set; } = "Consulta inicial";
    public int ClienteId { get; set; }
}

// DTOs/ActualizacionDTO.cs
public class ActualizacionDTO
{
    public string Contenido { get; set; } = string.Empty;
    public int CasoId { get; set; }
}