
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Business.DTOs.Subscriber;

namespace Botanical.ViewModels.HomeViewModels
{
    public class HomeVM
    {
        //public List<GetProductDTO>? Products { get; set; }
        public List<GetProductDTO>? Products { get; set; } = new();
        public List<GetSliderDTO>? Sliders { get; set; } = new();
        public List<GetSettingsDTO>? Settings { get; set; } = new();
        public List<GetCategoryDTO>? Categories { get; set; } = new();
        public GetSubscriberDTO Subscriber { get; set; } = new();
    }
}
