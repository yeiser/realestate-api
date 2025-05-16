using Application.Commands.Properties;
using Application.Validators.CustomRules;
using FluentValidation;

namespace Application.Validators.Properties
{
    public class AddPropertyImageValidator : AbstractValidator<AddPropertyImageCommand>
    {
        public AddPropertyImageValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.Base64Image).MustBeValidBase64Image(5);
        }
    }
}
