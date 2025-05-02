using AutoMapper;
using Business.DTOs.Products;
using Business.IServices;
using Core.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductServiceForPresentation : IProductServiceForPresentation
    {
        private readonly IProductReadRepository _ProductReadRepository;
        private readonly IProductWriteRepository _ProductWriteRepository;
        private readonly IValidator<CreateProductDTO> _CreateProductValidator;
        private readonly IMapper _mapper;
        //private readonly IFileService _fileService;
        public ProductServiceForPresentation(IProductReadRepository ProductReadRepository, IProductWriteRepository ProductWriteRepository, IValidator<CreateProductDTO> createProductValidator, IMapper mapper)
        {
            _ProductReadRepository = ProductReadRepository;
            _ProductWriteRepository = ProductWriteRepository;
            _CreateProductValidator = createProductValidator;
            _mapper = mapper;
        }
        public async Task AddToWishList(Guid Id)
        {
            var product = await _ProductReadRepository.GetByIdAsync(Id.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.IsInWishList = true;

            _ProductWriteRepository.Update(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }

        public async Task AddToCart(Guid Id)
        {
            var product = await _ProductReadRepository.GetByIdAsync(Id.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.IsInCart = true;

            _ProductWriteRepository.Update(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }


        public async Task<GetProductDTO> ProductDetail(Guid Id)
        {
            var productDetail = await _ProductReadRepository.GetAll().Where(c => c.Id == Id).Select(x => new GetProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                ImagePath = x.ImagePath,
                ProductCode = x.ProductCode,
                Price = x.Price,
                DiscountedPrice = x.DiscountedPrice,
                Description = x.Description,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId
            }).FirstOrDefaultAsync();

            return productDetail;
        }

        public async Task RemoveFromWishList(Guid Id)
        {
            var product = await _ProductReadRepository.GetByIdAsync(Id.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.IsInWishList = false;

            _ProductWriteRepository.Update(product);
            await _ProductWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetProductDTO>> Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return new List<GetProductDTO>();
            }

            var products = await _ProductReadRepository.GetAll()
                .Where(p => !p.IsDeleted)
                .Select(p => new GetProductDTO
                {
                    Name = p.Name,
                    ImagePath = p.ImagePath
                })
                .ToListAsync();

            return products.ToList();
        }
    }
}
