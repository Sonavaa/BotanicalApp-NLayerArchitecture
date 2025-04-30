using AutoMapper;
using Botanical.Models;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Product
            CreateMap<Product, CreateProductDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));
            //updatedBy elave et
            CreateMap<Product, GetProductDTO>().ReverseMap();

            //Category
            CreateMap<Category, CreateCategoryDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));
            //updatedBy elave et
            CreateMap<Category, GetCategoryDTO>().ReverseMap();

            //Slider
            CreateMap<Slider, CreateSliderDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));
            //updatedBy elave et
            CreateMap<Slider, GetSliderDTO>().ReverseMap();

            //Settings
            CreateMap<Setting, CreateSettingsDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));
            //updatedBy elave et
            CreateMap<Setting, GetSettingsDTO>().ReverseMap();
        }
    }
}
