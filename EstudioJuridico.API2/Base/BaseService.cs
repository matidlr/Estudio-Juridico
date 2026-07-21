namespace EstudioJuridico.API2.Base
{
    public abstract class BaseService
    {
        protected void ValidarRequerido(string valor, string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException($"El campo {campo} es requerido.");
        }

        protected void ValidarPositivo(decimal valor, string campo)
        {
            if (valor <= 0)
                throw new ArgumentException($"El campo {campo} debe ser mayor a cero.");
        }

        protected void ValidarFechaFutura(DateTime fecha, string campo)
        {
            if (fecha <= DateTime.UtcNow)
                throw new ArgumentException($"El campo {campo} debe ser una fecha futura.");
        }
    }
}