public class CasoDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string NombrePartes { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo";
    public string Etapa { get; set; } = "Consulta inicial";
    public int ClienteId { get; set; };
    public int? AbogadoId       { get; set; }
}