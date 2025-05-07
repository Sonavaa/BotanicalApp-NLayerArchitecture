using AutoMapper;
using Business.DTOs.Settings;
using Business.DTOs.Settings;
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
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsReadRepository _SettingsReadRepository;
        private readonly ISettingsWriteRepository _SettingsWriteRepository;
        private readonly IValidator<CreateSettingsDTO> _CreateSettingsValidator;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        public SettingsService(ISettingsReadRepository SettingsReadRepository, ISettingsWriteRepository SettingsWriteRepository, IValidator<CreateSettingsDTO> createSettingsValidator, IMapper mapper, IHostEnvironment env)
        {
            _SettingsReadRepository = SettingsReadRepository;
            _SettingsWriteRepository = SettingsWriteRepository;
            _CreateSettingsValidator = createSettingsValidator;
            _mapper = mapper;
            _env = env;
        }
        public async Task AddSetting(CreateSettingsDTO createSettingDTO)
        {
            bool isSettingExist = await _SettingsReadRepository.GetAll().AnyAsync(x => x.Key == createSettingDTO.Key || x.Value == createSettingDTO.Value);
            if (isSettingExist)
            {
                throw new BadRequestException("This Key already exists.");
            }

            //if (!createSettingDTO.ImageFile.CheckFileType("image"))
            //{
            //    throw new Exception("Invalid file type. Please upload an image.");
            //}

            //if (!createSettingDTO.ImageFile.CheckFileSize(200))
            //{
            //    throw new Exception("File size is too large. Maximum allowed size is 200MB.");
            //}

            string? uniqueFileName = await createSettingDTO.ImageFile
                .SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "SettingsImages");



            var Settings = _mapper.Map<Setting>(createSettingDTO);

            Settings.Id = Guid.NewGuid();
                Settings.ImgPath = uniqueFileName;

            await _SettingsWriteRepository.AddAsync(Settings);
            await _SettingsWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetSettingsDTO>> GetAllSettings()
        {
            var Settings = await _SettingsReadRepository.GetAll()
               .Select(s => new GetSettingsDTO
               {
                   Id = s.Id,
                   Key = s.Key,
                   Value = s.Value,
                   IsDeleted = s.IsDeleted,
                   Title = s.Title,
                   Description = s.Description,
                   ImgPath = s.ImgPath,
               })
       .ToListAsync();

            return Settings;
        }

        public async Task<GetSettingsDTO> GetSettingByKey(Guid Id)
        {
            var Setting = await _SettingsReadRepository.GetAll().Where(c => c.Id == Id).Select(x => new GetSettingsDTO
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
                Title = x.Title,
                Description = x.Description,
                IsDeleted= x.IsDeleted,
                ImgPath = x.ImgPath,
            }).FirstOrDefaultAsync();

            return Setting;
        }

        public async Task DeleteSetting(Guid Id)
        {
            var Setting = await _SettingsReadRepository.GetByIdAsync(Id.ToString());
            if (Setting == null)
            {
                throw new Exception("Setting not found");
            }
            Setting.SoftDelete();

            _SettingsWriteRepository.Update(Setting);
            await _SettingsWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreSetting(Guid Id)
        {
            var Setting = await _SettingsReadRepository.GetByIdAsync(Id.ToString());
            if (Setting == null)
            {
                throw new Exception("Setting not found");
            }
            Setting.Restore();

            _SettingsWriteRepository.Update(Setting);
            await _SettingsWriteRepository.SaveChangeAsync();
        }

        public async Task EditSetting(Guid Id, CreateSettingsDTO updateSettingDTO)
        {
            var Setting = await _SettingsReadRepository.GetAll()
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (Setting == null)
            {
                throw new Exception("Setting not found.");
            }

            if (updateSettingDTO.ImageFile != null)
            {
                if (!updateSettingDTO.ImageFile.CheckFileType("image"))
                {
                    throw new Exception("Invalid file type. Please upload an image.");
                }

                if (!updateSettingDTO.ImageFile.CheckFileSize(200))
                {
                    throw new Exception("File size is too large. Maximum allowed size is 200MB.");
                }

                if (!string.IsNullOrEmpty(Setting.ImgPath))
                {
                    string oldFileName = Path.GetFileName(Setting.ImgPath);
                    FileExtensions.DeleteFile(
                        _env.ContentRootPath,
                        "client",
                        "assets",
                        "images",
                        "SettingsImages",
                        oldFileName
                    );
                }

                string uniqueFileName = await updateSettingDTO.ImageFile
                    .SaveFilesAsync(_env.ContentRootPath, "client", "assets", "images", "SettingsImages");

                Setting.ImgPath = uniqueFileName;
            }
            else
            {
                Setting.ImgPath = Setting.ImgPath;
            }
            Setting.Key = updateSettingDTO.Key;
            Setting.Value = updateSettingDTO.Value; 
            Setting.Title = updateSettingDTO.Title;
            Setting.Description = updateSettingDTO.Description;
            Setting.UpdatedBy = "Admin";
            Setting.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _SettingsWriteRepository.Update(Setting);
            await _SettingsWriteRepository.SaveChangeAsync();
        }
    }
}
