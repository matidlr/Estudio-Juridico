public class Caso
{
    public int Id { get; set; }

    // Carátula: "García Juan c/ Empresa XYZ s/ Despido"
    public string Caratula { get; set; } = string.Empty;

    // Proceso: "Ordinario", "Sumarísimo", "Ejecutivo", etc.
    public string Proceso { get; set; } = string.Empty;

    // Juzgado interviniente
    public string Juzgado { get; set; } = string.Empty;

    // Número de expediente
    public string NroExpediente { get; set; } = string.Empty;

    // Tipo: Laboral, Civil, Penal, etc.
    public string Tipo { get; set; } = string.Empty;

    public string Estado { get; set; } = "Activo";
    public string Etapa  { get; set; } = "Consulta inicial";

    public DateTime FechaInicio  { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }

    public int ClienteId  { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int AbogadoId  { get; set; }
    public Abogado Abogado { get; set; } = null!;

    public List<Actualizacion> Actualizaciones { get; set; } = new();
    public List<Archivo> Archivos              { get; set; } = new();
    public List<Prueba> Pruebas                { get; set; } = new();
    public List<Comentario> Comentarios        { get; set; } = new();
}