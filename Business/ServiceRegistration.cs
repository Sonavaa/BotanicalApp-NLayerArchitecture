﻿using Business.IServices;
using Business.Services;
using Business.Validators.Category;
using Business.Validators.Contact;
using Business.Validators.Product;
using Business.Validators.Settings;
using Business.Validators.Slider;
using Business.Validators.User;
using Core.Repositories;
using Data.Repositories;
using Data.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<CreateProductDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCategoryDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateSliderDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateSettingsDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateContactDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IProductServiceForPresentation, ProductServiceForPresentation>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IWishlistService, WishlistService>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            services.AddScoped<ISliderReadRepository, SliderReadRepository>();
            services.AddScoped<ISliderWriteRepository, SliderWriteRepository>();
            services.AddScoped<ISettingsReadRepository, SettingsReadRepository>();
            services.AddScoped<ISettingsWriteRepository, SettingsWriteRepository>();
            services.AddScoped<IContactReadRepository, ContactReadRepository>();
            services.AddScoped<IContactWriteRepository, ContactWriteRepository>();
            services.AddScoped<ITagsReadRepository, TagsReadRepository>();
            services.AddScoped<ITagsWriteRepository, TagsWriteRepository>();
            services.AddScoped<IProductTagReadRepository, ProductTagReadRepository>();
            services.AddScoped<IProductTagWriteRepository, ProductTagWriteRepository>();
            services.AddScoped<ISubscriberReadRepository, SubscriberReadRepository>();
            services.AddScoped<ISubscriberWriteRepository, SubscriberWriteRepository>();
            services.AddScoped<IWishlistItemReadRepository, WishlistItemReadRepository>();
            services.AddScoped<IWishlistItemWriteRepository, WishlistItemWriteRepository>();
        }
    }
}
