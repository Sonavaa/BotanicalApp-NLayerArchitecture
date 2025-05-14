
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Business.DTOs.Tag;
using Core.Entities;

namespace Botanical.ViewModels.StoreViewModels
{
    public class StoreVM
    {
        public List<GetProductDTO>? Products { get; set; } = new();
        public List<GetSettingsDTO>? Settings { get; set; } = new();
        public List<GetProductTagDTO>? Tags { get; set; } = new();
        public List<GetCategoryDTO>? Categories { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
