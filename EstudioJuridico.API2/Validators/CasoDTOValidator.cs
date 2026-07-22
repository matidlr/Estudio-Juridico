using FluentValidation;

namespace EstudioJuridico.API2.Validators
{
    public class CasoDTOValidator : AbstractValidator<CasoDTO>
    {
        public CasoDTOValidator()
        {
            RuleFor(x => x.Caratula)
                .NotEmpty().WithMessage("La carátula es obligatoria.")
                .MaximumLength(500).WithMessage("La carátula no puede superar los 500 caracteres.");

            RuleFor(x => x.Tipo)
                .NotEmpty().WithMessage("El tipo es obligatorio.")
                .Must(t => new[] { "Laboral", "Civil", "Penal", "Familia", "Comercial" }.Contains(t))
                .WithMessage("El tipo debe ser Laboral, Civil, Penal, Familia o Comercial.");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .Must(e => new[] { "Activo", "Suspendido", "Finalizado", "Archivado" }.Contains(e))
                .WithMessage("El estado no es válido.");

            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("El cliente es obligatorio.");
        }
    }
}