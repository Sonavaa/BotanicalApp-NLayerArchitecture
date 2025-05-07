using Business.DTOs.Contact;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Contact
{
    public class CreateContactDTOValidator : AbstractValidator<CreateContactDTO>
    {
        public CreateContactDTOValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key is required.")
                .MaximumLength(100).WithMessage("Key must not exceed 100 characters.");

            RuleFor(x => x.Values)
                .NotNull().WithMessage("Value list cannot be null.")
                .Must(x => x!.All(v => !string.IsNullOrWhiteSpace(v) && v.Length >= 2))
                .WithMessage("Value must be at least 2 characters long and not empty.");
        }

    }
}
