using Application.Commands.Properties;
using FluentValidation;

namespace Application.Validators.Properties
{
    public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyValidator()
        {
            RuleFor(x => x.IdProperty).NotEmpty().WithMessage("El de la propiedad es obligatorio.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
        }
    }
}
