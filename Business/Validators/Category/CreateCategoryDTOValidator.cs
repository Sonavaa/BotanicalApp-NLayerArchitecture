using FluentValidation;
using Business.DTOs.Category;
using Business.Extensions;

namespace Business.Validators.Category
{
    public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category Name is required.")
                .Length(3, 100).WithMessage("Category Name must be between 3 and 100 characters.");

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
