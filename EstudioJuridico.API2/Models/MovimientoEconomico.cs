using EstudioJuridico.API2.Base;

public class MovimientoEconomico : BaseEntity
{


    // Tipo: "Honorario", "Gasto", "Pago"
    public string Tipo { get; set; } = string.Empty;

    // Concepto: "Honorarios iniciales", "Tasa judicial", "Pago cliente", etc.
    public string Concepto { get; set; } = string.Empty;

    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Forma de pago (solo para pagos)
    public string? FormaPago { get; set; }

    public string? Notas { get; set; }

    public int CasoId { get; set; }
    public Caso Caso { get; set; } = null!;
}