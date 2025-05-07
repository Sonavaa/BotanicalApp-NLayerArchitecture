using AutoMapper;
using Business.DTOs.Slider;
using Business.DTOs.Slider;
using Business.DTOs.Slider;
using Business.Extensions;
using Business.IServices;
using Core.Entities;
using Core.Repositories;
using Data.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.SecurityTokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderReadRepository _SliderReadRepository;
        private readonly ISliderWriteRepository _SliderWriteRepository;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        public SliderService(ISliderReadRepository SliderReadRepository, ISliderWriteRepository SliderWriteRepository, IMapper mapper, IHostEnvironment env)
        {
            _SliderReadRepository = SliderReadRepository;
            _SliderWriteRepository = SliderWriteRepository;
            _mapper = mapper;
            _env = env;
        }
        public async Task AddSlider(CreateSliderDTO addSliderDTO)
        {
            bool isSliderExist = await _SliderReadRepository.GetAll().AnyAsync(x => x.Title == addSliderDTO.Title || x.Id == addSliderDTO.Id);
            if (isSliderExist)
            {
                throw new BadRequestException("This slider already exists.");
            }

            if (!addSliderDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addSliderDTO.ImageFile.CheckFileSize(200))
            {
                throw new Exception("File size is too large. Maximum allowed size is 200MB.");
            }

            string uniqueFileName = await addSliderDTO.ImageFile
                .SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "SliderImages");


            var Slider = _mapper.Map<Slider>(addSliderDTO);

            Slider.Id = Guid.NewGuid();
            Slider.ImagePath = uniqueFileName;

            await _SliderWriteRepository.AddAsync(Slider);
            await _SliderWriteRepository.SaveChangeAsync();
        }

        public async Task DeleteSlider(Guid Id)
        {
            var Slider = await _SliderReadRepository.GetByIdAsync(Id.ToString());
            if (Slider == null)
            {
                throw new Exception("Slider not found");
            }
            Slider.SoftDelete();

            _SliderWriteRepository.Update(Slider);
            await _SliderWriteRepository.SaveChangeAsync();
        }

        public async Task EditSlider(Guid Id, CreateSliderDTO updateSliderDTO)
        {
            var Slider = await _SliderReadRepository.GetAll()
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (Slider == null)
            {
                throw new Exception("Slider not found.");
            }

            if (updateSliderDTO.ImageFile != null)
            {
                if (!updateSliderDTO.ImageFile.CheckFileType("image"))
                {
                    throw new Exception("Invalid file type. Please upload an image.");
                }

                if (!updateSliderDTO.ImageFile.CheckFileSize(200))
                {
                    throw new Exception("File size is too large. Maximum allowed size is 200MB.");
                }

                if (!string.IsNullOrEmpty(Slider.ImagePath))
                {
                    string oldFileName = Path.GetFileName(Slider.ImagePath);
                    FileExtensions.DeleteFile(
                        _env.ContentRootPath,
                        "client",
                        "assets",
                        "images",
                        "SliderImages",
                        oldFileName
                    );
                }

                string uniqueFileName = await updateSliderDTO.ImageFile
                    .SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "SliderImages");

                Slider.ImagePath = uniqueFileName;
            }
            else
            {
                Slider.ImagePath = Slider.ImagePath;
            }

            Slider.Title = updateSliderDTO.Title;
            Slider.Description = updateSliderDTO.Description;
            Slider.SubTitle = updateSliderDTO.SubTitle;
            Slider.ImagePath = updateSliderDTO.ImagePath;
            Slider.UpdatedBy = "Admin";
            Slider.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _SliderWriteRepository.Update(Slider);
            await _SliderWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetSliderDTO>> GetAllSliderAsync()
        {
            var Sliders = await _SliderReadRepository.GetAll()
                .Select(p => new GetSliderDTO
                {
                    Id = p.Id,
                    IsDeleted = p.IsDeleted,
                    Title = p.Title,
                    SubTitle = p.SubTitle,
                    Description = p.Description,
                    ImagePath = p.ImagePath,
                    //CreatedAt = DateTime.UtcNow.AddHours(4),
                })
        .ToListAsync();


            return Sliders;
        }

        public async Task<GetSliderDTO> GetSliderById(Guid Id)
        {
            var Slider = await _SliderReadRepository.GetAll().Where(c => c.Id == Id).Select(x => new GetSliderDTO
            {
                Id = x.Id,
                Title = x.Title,
                SubTitle = x.SubTitle,
                Description = x.Description,
            }).FirstOrDefaultAsync();
            return Slider;
        }

        public async Task RestoreSlider(Guid Id)
        {
            var Slider = await _SliderReadRepository.GetByIdAsync(Id.ToString());
            if (Slider == null)
            {
                throw new Exception("Slider not found");
            }
            Slider.Restore();

            _SliderWriteRepository.Update(Slider);
            await _SliderWriteRepository.SaveChangeAsync();
        }
    }
}
