using AutoMapper;
using Botanical.Models;
using Business.DTOs.Account;
using Business.DTOs.Category;
using Business.DTOs.Contact;
using Business.DTOs.Products;
using Business.DTOs.Settings;
using Business.DTOs.Slider;
using Core.Entities;
using Core.Entities.Identity;
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
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)))
                  .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                  .ForMember(dest => dest.IsInWishList, opt => opt.MapFrom(src => false))
                  .ForMember(dest => dest.IsInCart, opt => opt.MapFrom(src => false));
            CreateMap<Product, GetProductDTO>().ReverseMap();

            //Category
            CreateMap<Category, CreateCategoryDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)))
                  .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            CreateMap<Category, GetCategoryDTO>().ReverseMap();

            //Slider
            CreateMap<Slider, CreateSliderDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)))
                  .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            CreateMap<Slider, GetSliderDTO>().ReverseMap();

            //Settings
            CreateMap<Setting, CreateSettingsDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)))
                  .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            CreateMap<Setting, GetSettingsDTO>().ReverseMap();

            //Contacts
            CreateMap<Contact, CreateContactDTO>().ReverseMap()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)))
                  .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                  .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            CreateMap<Contact, GetContactDTO>().ReverseMap();

            //User
            CreateMap<UserRegisterDTO, AppUser>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Firstname))
              .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Lastname))
              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
