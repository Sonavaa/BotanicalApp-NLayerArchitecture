using AutoMapper;
using Botanical.Models;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.Extensions;
using Business.Services;
using Core.Entities;
using Core.Repositories;
using Data.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Data.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _ProductReadRepository;
        private readonly IProductWriteRepository _ProductWriteRepository;
        private readonly IValidator<CreateProductDTO> _CreateProductValidator;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        //private readonly IFileService _fileService;
        public ProductService(IProductReadRepository ProductReadRepository, IProductWriteRepository ProductWriteRepository, IValidator<CreateProductDTO> createProductValidator, IMapper mapper, IHostEnvironment env)
        {
            _ProductReadRepository = ProductReadRepository;
            _ProductWriteRepository = ProductWriteRepository;
            _CreateProductValidator = createProductValidator;
            _mapper = mapper;
            _env = env;
        }

        public async Task AddProduct(CreateProductDTO addProductDTO)
        {
            bool isProductExist = await _ProductReadRepository.GetAll().AnyAsync(x => (x.Name == addProductDTO.Name && x.CategoryId == addProductDTO.CategoryId)
                || x.ProductCode == addProductDTO.ProductCode);
            if (isProductExist)
            {
                throw new BadRequestException("This product already exists.");
            }


            if (!addProductDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addProductDTO.ImageFile.CheckFileSize(200))
            {
                throw new Exception("File size is too large. Maximum allowed size is 200MB.");
            }

            string uniqueFileName = await addProductDTO.ImageFile.SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "ProductImages");


            var product = _mapper.Map<Product>(addProductDTO);

            product.Id = Guid.NewGuid();
            product.ImagePath = uniqueFileName;
            product.IsInWishList = false;
            product.IsInCart = false;
            product.CreatedBy = "System";

            await _ProductWriteRepository.AddAsync(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetProductDTO>> GetAllProductAsync()
        {
            var products = await _ProductReadRepository.GetAll()
                 .Select(p => new GetProductDTO
                 {
                     Id = p.Id,
                     IsDeleted = p.IsDeleted,
                     ImagePath = p.ImagePath,
                     Name = p.Name,
                     Price = p.Price,
                     DiscountedPrice = p.DiscountedPrice,
                     Description = p.Description,
                     ProductCode = p.ProductCode,
                     Quantity = p.Quantity,
                     IsInWishList = (bool)p.IsInWishList,
                     IsInCart = (bool)p.IsInCart,
                     CreatedAt = DateTime.UtcNow.AddHours(4),
                 })
         .ToListAsync();


            return products;
        }
        public async Task<GetProductDTO> GetProductById(Guid Id)
        {

            var product = await _ProductReadRepository.GetAll().Where(c => c.Id == Id).Select(x => new GetProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                ImagePath = x.ImagePath,
                ProductCode = x.ProductCode,
                Price = x.Price,
                DiscountedPrice = x.DiscountedPrice,
                Description = x.Description,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId,
                IsInWishList = (bool)x.IsInWishList,
                IsInCart = (bool)x.IsInCart
            }).FirstOrDefaultAsync();
            return product;
        }

        public async Task EditProduct(Guid Id, CreateProductDTO updateProductDTO)
        {

            var validationResult = await _CreateProductValidator.ValidateAsync(updateProductDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = await _ProductReadRepository.GetAll()
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (category == null)
            {
                throw new Exception("Product not found.");
            }

            if (updateProductDTO.ImageFile != null)
            {
                if (!updateProductDTO.ImageFile.CheckFileType("image"))
                {
                    throw new Exception("Invalid file type. Please upload an image.");
                }

                if (!updateProductDTO.ImageFile.CheckFileSize(200))
                {
                    throw new Exception("File size is too large. Maximum allowed size is 200MB.");
                }

                if (!string.IsNullOrEmpty(category.ImagePath))
                {
                    string oldFileName = Path.GetFileName(category.ImagePath);
                    FileExtensions.DeleteFile(
                        _env.ContentRootPath,
                        "client",
                        "assets",
                        "images",
                        "ProductImages",
                        oldFileName
                    );
                }

                string uniqueFileName = await updateProductDTO.ImageFile
                    .SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "ProductImages");

                category.ImagePath = uniqueFileName;
            }
            else
            {
                category.ImagePath = category.ImagePath;
            }

            category.Name = updateProductDTO.Name;
            category.UpdatedBy = "Admin";
            category.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _ProductWriteRepository.Update(category);
            await _ProductWriteRepository.SaveChangeAsync();
        }

        public async Task DeleteProduct(Guid Id)
        {
            var product = await _ProductReadRepository.GetByIdAsync(Id.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.SoftDelete();

            _ProductWriteRepository.Update(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreProduct(Guid Id)
        {
            var product = await _ProductReadRepository.GetByIdAsync(Id.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.Restore();

            _ProductWriteRepository.Update(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }
    }
}
