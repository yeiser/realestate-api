using Application.Commands.Owners;
using FluentValidation;
using Application.Validators.CustomRules;

namespace Application.Validators.Owners
{
    public class CreateOwnerValidator : AbstractValidator<CreateOwnerCommand>
    {
        public CreateOwnerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Photo).MustBeValidBase64Image(5);
            RuleFor(x => x.Birthday).LessThan(DateTime.Now);
        }
    }
}
