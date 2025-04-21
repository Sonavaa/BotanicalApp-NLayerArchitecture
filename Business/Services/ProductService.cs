using AutoMapper;
using Botanical.Models;
using Business.DTOs.Products;
using Business.Extensions;
using Business.Services;
using Core.Repositories;
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
            //bool isProductExist = await _ProductReadRepository.GetAll().AnyAsync(x => x.Name == addProductDTO.Name && x.Id == addProductDTO.Id);
            //if (isProductExist)
            //{
            //    throw new BadRequestException("This product already exists.");
            //}

            var validationResult = await _CreateProductValidator.ValidateAsync(addProductDTO);

            if (!addProductDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addProductDTO.ImageFile.CheckFileSize(200))
            {
                throw new Exception("File size is too large. Maximum allowed size is 200MB.");
            }

            string uniqueFileName = await addProductDTO.ImageFile.SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "productImages");


            var product = _mapper.Map<Product>(addProductDTO);

            product.Id = Guid.NewGuid();
            product.ImagePath = uniqueFileName;
            product.IsInWishList = false;
            product.CreatedBy = "System";
            product.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _ProductWriteRepository.AddAsync(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetProductDTO>> GetAllProductAsync()
        {
            var products = await _ProductReadRepository.GetAll()
                 .Select(p => new GetProductDTO
                 {
                     Name = p.Name,
                     Price = p.Price,
                     DiscountedPrice = p.DiscountedPrice,
                     Description = p.Description,
                     ProductCode = p.ProductCode,
                     Quantity = p.Quantity,
                     //Images = List<ProductImages>{ },
                     CreatedAt = DateTime.UtcNow.AddHours(4),
                 })
         .ToListAsync();


            return products;
        }

        public async Task GetProductById(Guid Id)
        {
            var product = await _ProductReadRepository.GetByIdAsync(Id.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }
        }

        public async Task EditProduct(Guid Id, CreateProductDTO updateProductDTO)
        {

            var validationResult = await _CreateProductValidator.ValidateAsync(updateProductDTO);
            throw new NotImplementedException();
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
