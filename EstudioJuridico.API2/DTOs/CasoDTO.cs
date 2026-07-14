public class CasoDTO
{
    public string Caratula      { get; set; } = string.Empty;
    public string Proceso       { get; set; } = string.Empty;
    public string Juzgado       { get; set; } = string.Empty;
    public string NroExpediente { get; set; } = string.Empty;
    public string Tipo          { get; set; } = string.Empty;
    public string Estado        { get; set; } = "Activo";
    public string Etapa         { get; set; } = "Consulta inicial";
    public int    ClienteId     { get; set; }
    public int?   AbogadoId     { get; set; }
}