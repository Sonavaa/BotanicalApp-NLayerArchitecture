using Business.DTOs.Products;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Product
{
    public class CreateProductDTOValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product Name is required.")
                .Length(3, 100).WithMessage("Product Name must be between 3 and 100 characters.");

            RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.DiscountedPrice)
                .LessThan(x => x.Price).When(x => x.DiscountedPrice.HasValue)
                .WithMessage("Discounted price must be less than the original price.");

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Product Description is required.")
               .Length(20, 500).WithMessage("Product Description must be between 20 and 500 characters.");

            RuleFor(x => x.ProductCode)
               .NotEmpty().WithMessage("Product code is required.")
               .Matches("^[A-Z]{2}\\d{3}$").WithMessage("Product code must follow the format 'FA008'.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

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
