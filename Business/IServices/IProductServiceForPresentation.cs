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
        Task AddToWishList(Guid Id);
        Task RemoveFromWishList(Guid Id);
        Task<GetProductDTO> ProductDetail(Guid Id);
        Task<List<GetProductDTO>> Search(string search);
    }
}
