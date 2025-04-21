using Business.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ICategoryService
    {
        Task AddCategory(CreateCategoryDTO addCategoryDTO);
        Task<List<GetCategoryDTO>> GetAllCategoryAsync();
        Task<GetCategoryDTO> GetCategoryById(Guid Id);
        Task EditCategory(Guid Id, CreateCategoryDTO updateCategoryDTO);
        Task DeleteCategory(Guid Id);
        Task RestoreCategory(Guid Id);
    }
}
