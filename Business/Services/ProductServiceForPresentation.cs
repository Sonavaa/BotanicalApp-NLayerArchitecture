using AutoMapper;
using Botanical.Models;
using Business.DTOs.Products;
using Business.IServices;
using Core.Entities;
using Core.Repositories;
using Data.Services;
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
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductDTO> _CreateProductValidator;
        private readonly IMapper _mapper;
        //private readonly IFileService _fileService;
        public ProductServiceForPresentation(IProductReadRepository ProductReadRepository, IProductWriteRepository ProductWriteRepository,
            IProductService productService, IValidator<CreateProductDTO> createProductValidator, IMapper mapper)
        {
            _ProductReadRepository = ProductReadRepository;
            _ProductWriteRepository = ProductWriteRepository;
            _productService = productService;
            _CreateProductValidator = createProductValidator;
            _mapper = mapper;
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
                    Id = p.Id,
                    Name = p.Name,
                    ImagePath = p.ImagePath
                })
                .ToListAsync();

            return products.ToList();
        }

        public async Task<List<GetProductDTO>> GetSortedProductsAsync(string sort, string category, string tag, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productService.GetAllProductAsync();


            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.categoryName == category).ToList();

            if (!string.IsNullOrEmpty(tag))
                products = products.Where(p => p.tagName == tag).ToList();

            if (minPrice.HasValue)
                products = products.Where(p => (p.DiscountedPrice ?? p.Price) >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                products = products.Where(p => (p.DiscountedPrice ?? p.Price) <= maxPrice.Value).ToList();

            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.DiscountedPrice ?? p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.DiscountedPrice ?? p.Price).ToList(),
                "top_rated" => products.OrderBy(p => p.CreatedAt).Where(p => p.IsInWishList).ToList(),
                "new_arr" => products.OrderByDescending(p => p.CreatedAt).ToList(),
                _ => products
            };

            return products;
        }

    }
}
