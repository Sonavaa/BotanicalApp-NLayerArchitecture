using AutoMapper;
using Botanical.Models;
using Business.DTOs.Category;
using Business.DTOs.Products;
using Business.Extensions;
using Business.IServices;
using Business.Services;
using Core.Entities;
using Core.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Data.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryReadRepository _CategoryReadRepository;
        private readonly ICategoryWriteRepository _CategoryWriteRepository;
        private readonly IValidator<CreateCategoryDTO> _CreateCategoryValidator;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        //private readonly IFileService _fileService;
        public CategoryService(ICategoryReadRepository CategoryReadRepository, ICategoryWriteRepository CategoryWriteRepository, IValidator<CreateCategoryDTO> createCategoryValidator, IMapper mapper, IHostEnvironment env)
        {
            _CategoryReadRepository = CategoryReadRepository;
            _CategoryWriteRepository = CategoryWriteRepository;
            _CreateCategoryValidator = createCategoryValidator;
            _mapper = mapper;
            _env = env;
        }

        public async Task AddCategory(CreateCategoryDTO addCategoryDTO)
        {
            //bool isCategoryExist = await _CategoryReadRepository.GetAll().AnyAsync(x => x.Name == addCategoryDTO.Name && x.Id == addCategoryDTO.Id);
            //if (isCategoryExist)
            //{
            //    throw new BadRequestException("This Category already exists.");
            //}

            var validationResult = await _CreateCategoryValidator.ValidateAsync(addCategoryDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (!addCategoryDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addCategoryDTO.ImageFile.CheckFileSize(200))
            {
                throw new Exception("File size is too large. Maximum allowed size is 200MB.");
            }

            string uniqueFileName = await addCategoryDTO.ImageFile.SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "CategoryImages");


            var Category = _mapper.Map<Category>(addCategoryDTO);

            Category.Id = Guid.NewGuid();
            Category.ImgPath = uniqueFileName;
            Category.CreatedBy = "System";
            Category.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _CategoryWriteRepository.AddAsync(Category);
            await _CategoryWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetCategoryDTO>> GetAllCategoryAsync()
        {
            var Categorys = await _CategoryReadRepository.GetAll()
                 .Select(p => new GetCategoryDTO
                 {
                     Id = p.Id,
                     IsDeleted = p.IsDeleted,
                     Name = p.Name,
                     ImgPath = p.ImgPath,
                     CreatedAt = DateTime.UtcNow.AddHours(4),
                 })
         .ToListAsync();


            return Categorys;
        }

        public async Task<GetCategoryDTO> GetCategoryById(Guid Id)
        {
            var Category = await _CategoryReadRepository.GetAll().Where(c=>c.Id == Id).Select(x => new GetCategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                ImgPath = x.ImgPath,
            }).FirstOrDefaultAsync();
            return Category;
        }

        public async Task EditCategory(Guid Id, CreateCategoryDTO updateCategoryDTO)
        {
            var validationResult = await _CreateCategoryValidator.ValidateAsync(updateCategoryDTO);
            var category = await _CategoryReadRepository.GetAll()
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            category.ImgPath = updateCategoryDTO.ImgPath;
            category.Name = updateCategoryDTO.Name;

            _CategoryWriteRepository.Update(category);
            await _CategoryWriteRepository.SaveChangeAsync();
        }


        public async Task DeleteCategory(Guid Id)
        {
            var Category = await _CategoryReadRepository.GetByIdAsync(Id.ToString());
            if (Category == null)
            {
                throw new Exception("Category not found");
            }
            Category.SoftDelete();

            _CategoryWriteRepository.Update(Category);
            await _CategoryWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreCategory(Guid Id)
        {
            var Category = await _CategoryReadRepository.GetByIdAsync(Id.ToString());
            if (Category == null)
            {
                throw new Exception("Category not found");
            }
            Category.Restore();

            _CategoryWriteRepository.Update(Category);
            await _CategoryWriteRepository.SaveChangeAsync();
        }
    }
}
