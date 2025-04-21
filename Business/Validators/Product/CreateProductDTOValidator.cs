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

            //isCreatingde elave et 

            //RuleFor(x => x.ImageFile)
            //    .NotNull().WithMessage("Image file is required.")
            //    .Must(file => file.CheckFileType("image")).WithMessage("Invalid file type. Please upload an image.")
            //    .Must(file => file.CheckFileSize(10)).WithMessage("File size is too large. Maximum allowed size is 10MB.");

      //      RuleFor(x => x.ImageFile)
      //.Must(BeAValidImage).When(x => x.ImageFile != null)
      //.WithMessage("Only image files (jpg, jpeg, png) are allowed.");
      //  }

      //  private bool BeAValidImage(IFormFile file)
      //  {
      //      var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
      //      var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
      //      return allowedExtensions.Contains(extension);
      //  }

    }
    }
}
