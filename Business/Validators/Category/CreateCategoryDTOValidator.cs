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
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Image file cannot be empty if provided.")
                .Must(file => file == null || file.CheckFileType("image")) 
                .WithMessage("Invalid file type. Please upload an image.")
                .Must(file => file == null || file.CheckFileSize(200)) 
                .WithMessage("File size is too large. Maximum allowed size is 200MB.");
        }
    }
}
