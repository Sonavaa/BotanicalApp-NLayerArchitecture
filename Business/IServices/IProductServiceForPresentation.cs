﻿using Botanical.Models;
using Business.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IProductServiceForPresentation
    {
        Task<GetProductDTO> ProductDetail(Guid Id);
        Task<List<GetProductDTO>> Search(string search);
        Task<List<GetProductDTO>> GetSortedProductsAsync(string sort, string category, string tag, decimal? minPrice, decimal? maxPrice);
    }
}
