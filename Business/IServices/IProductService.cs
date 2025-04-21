using Business.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IProductService
    {   
        Task AddProduct(CreateProductDTO addProductDTO);
        Task<List<GetProductDTO>> GetAllProductAsync();
        Task GetProductById(Guid Id);
        Task EditProduct(Guid Id, CreateProductDTO updateProductDTO);
        Task DeleteProduct(Guid Id);
        Task RestoreProduct(Guid Id);
    }
}
