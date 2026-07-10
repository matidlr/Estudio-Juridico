public class ComentarioDTO
{
    public string Texto           { get; set; } = string.Empty;
    public int    CasoId          { get; set; }
    public bool   VisibleAlAbogado { get; set; } = true;
}