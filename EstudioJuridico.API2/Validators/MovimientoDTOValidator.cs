using FluentValidation;

namespace EstudioJuridico.API2.Validators
{
    public class MovimientoDTOValidator : AbstractValidator<MovimientoDTO>
    {
        public MovimientoDTOValidator()
        {
            RuleFor(x => x.Tipo)
                .NotEmpty().WithMessage("El tipo es obligatorio.")
                .Must(t => new[] { "Honorario", "Gasto", "Pago" }.Contains(t))
                .WithMessage("El tipo debe ser Honorario, Gasto o Pago.");

            RuleFor(x => x.Concepto)
                .NotEmpty().WithMessage("El concepto es obligatorio.")
                .MaximumLength(300).WithMessage("El concepto no puede superar los 300 caracteres.");

            RuleFor(x => x.Monto)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

            RuleFor(x => x.CasoId)
                .GreaterThan(0).WithMessage("El caso es obligatorio.");
        }
    }
}