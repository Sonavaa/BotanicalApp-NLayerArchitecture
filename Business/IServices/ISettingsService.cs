using Business.DTOs.Settings;
using Business.DTOs.Slider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ISettingsService
    {
        Task AddSetting(CreateSettingsDTO createSettingDTO);
        Task<List<GetSettingsDTO>> GetAllSettings();
        Task<GetSettingsDTO> GetSettingByKey(Guid Id);
        Task EditSetting(Guid Id, CreateSettingsDTO updateSettingDTO);
        Task DeleteSetting(Guid Id);
        Task RestoreSetting(Guid Id);
    }
}
