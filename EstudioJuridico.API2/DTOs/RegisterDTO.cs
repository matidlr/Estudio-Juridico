public class RegisterDTO
{
    public string Nombre       { get; set; } = string.Empty;
    public string Apellido     { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;
    public string Password     { get; set; } = string.Empty;
    public string Telefono     { get; set; } = string.Empty;
    public string? Matricula   { get; set; }
    public string? Especialidad { get; set; }
}