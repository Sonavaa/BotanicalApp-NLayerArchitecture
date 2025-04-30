using Business.DTOs.Settings;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Settings
{
    public class CreateSettingsDTOValidator : AbstractValidator<CreateSettingsDTO>
    {
        public CreateSettingsDTOValidator()
        {
            RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .Length(2, 50).WithMessage("Key must be between 2 and 50 characters.");

            RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required.")
            .Length(2, 500).WithMessage("Value must be between 2 and 500 characters.");

            RuleFor(x => x.Title)
            .Length(10, 200).WithMessage("Title must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
           .Length(2, 500).WithMessage("Description must be between 3 and 100 characters.");

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length <= 200 * 1024 * 1024)
                    .WithMessage("Image size must be less than 200MB.")
                .Must(file => file == null ||
                    file.ContentType == "image/jpeg" ||
                    file.ContentType == "image/png" ||
                    file.ContentType == "image/jpg")
                    .WithMessage("Only JPEG, JPG, and PNG formats are allowed.");
        }
    }
}
