using Business.DTOs.Slider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ISliderService
    {
        Task AddSlider(CreateSliderDTO addSliderDTO);
        Task<List<GetSliderDTO>> GetAllSliderAsync();
        Task<GetSliderDTO> GetSliderById(Guid Id);
        Task EditSlider(Guid Id, CreateSliderDTO updateSliderDTO);
        Task DeleteSlider(Guid Id);
        Task RestoreSlider(Guid Id);
    }
}
