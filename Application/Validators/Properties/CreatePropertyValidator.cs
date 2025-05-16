using Application.Commands.Properties;
using Application.Validators.CustomRules;
using FluentValidation;

namespace Application.Validators.Properties
{
    public class CreatePropertyValidator : AbstractValidator<CreatePropertyWithImageCommand>
    {
        public CreatePropertyValidator()
        {

            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ImageBase64).MustBeValidBase64Image(5);
        }
    }
}
