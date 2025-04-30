using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Settings
{
    public class CreateSettingsDTO
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImgPath { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
