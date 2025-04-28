using Business.DTOs.Slider;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Slider
{
    public class CreateSliderDTOValidator : AbstractValidator<CreateSliderDTO>
    {
        public CreateSliderDTOValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.SubTitle)
                .MaximumLength(150).WithMessage("Subtitle must not exceed 150 characters.")
                .When(x => !string.IsNullOrEmpty(x.SubTitle));

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.ImageFile)
                .NotEmpty().WithMessage("Image is required.");

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length > 0).WithMessage("Uploaded image cannot be empty.")
                .Must(file => file == null || file.Length <= 200 * 1024 * 1024) 
                    .WithMessage("Image size must be less than 2MB.")
                .Must(file => file == null ||
                    file.ContentType == "image/jpeg" ||
                    file.ContentType == "image/png" ||
                    file.ContentType == "image/jpg")
                    .WithMessage("Only JPEG, JPG, and PNG formats are allowed.");
        }
    }
}
