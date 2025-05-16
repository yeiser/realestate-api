using Application.Commands.Owners;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Owners
{
    public class UpdateOwnerValidator : AbstractValidator<UpdateOwnerCommand>
    {
        public UpdateOwnerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Birthday).LessThan(DateTime.Now);
        }
    }
}
