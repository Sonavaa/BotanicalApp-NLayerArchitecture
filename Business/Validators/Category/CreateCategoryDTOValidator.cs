using Business.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Category
{
    public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryDTOValidator() 
        {
            RuleFor(x => x.Name)
                  .NotEmpty().WithMessage("Category Name is required.")
                  .Length(3, 100).WithMessage("Category Name must be between 3 and 100 characters.");
        }
    }
}
