
public class Caso
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string NombrePartes { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    // Tipo de caso: "Laboral", "Civil", "Penal", "Familia", "Comercial"
    public string Tipo { get; set; } = string.Empty;

    // Estado actual: "Activo", "Suspendido", "Finalizado", "Archivado"
    public string Estado { get; set; } = "Activo";

    // Etapa del juicio: "Consulta inicial", "Audiencia", etc.
    public string Etapa { get; set; } = "Consulta inicial";

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }

    // FK hacia el cliente dueño del caso
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    // FK hacia el abogado que lo gestiona
    public int AbogadoId { get; set; }
    public Abogado Abogado { get; set; } = null!;

    // Navegación: un caso tiene muchas actualizaciones, archivos, etc.
    public List<Actualizacion> Actualizaciones { get; set; } = new();
    public List<Archivo> Archivos { get; set; } = new();
    public List<Prueba> Pruebas { get; set; } = new();
    public List<Comentario> Comentarios { get; set; } = new();
}