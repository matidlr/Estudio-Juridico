using System.ComponentModel.DataAnnotations;

public class ActualizacionDTO
{
    [Required]
    [MaxLength(50000, ErrorMessage = "El contenido no puede superar los 50.000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;

    [Required]
    public int CasoId { get; set; }

    [MaxLength(20, ErrorMessage = "El número de foja no puede superar los 20 caracteres.")]
    public string? NroFoja { get; set; }

    [MaxLength(10000, ErrorMessage = "La aclaración no puede superar los 10.000 caracteres.")]
    public string? AclaracionCliente { get; set; }
}