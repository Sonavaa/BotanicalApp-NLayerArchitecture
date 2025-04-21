﻿//using Business.IServices;
//using Microsoft.AspNetCore.Http;
//using SixLabors.ImageSharp.Formats.Png;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;

//namespace Business.Services
//{
//    public class FileService : IFileService
//    {
//        private readonly IWebHostEnvironment _environment;

//        public FileService(IWebHostEnvironment environment)
//        {
//            _environment = environment;
//        }

//        public void Delete(string path)
//        {
//            var fullPath = Path.Combine(_environment.WebRootPath, path);
//            if (e::File.Exists(path))
//                e::File.Delete(path);
//        }

//        public async Task<string> SaveFilesAsync(IFormFile file, string webRootPath, params string[] subfolders)
//        {
//            if (file == null || file.Length == 0)
//                return null;

//            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

//            string folderPath = Path.Combine(webRootPath, Path.Combine(subfolders));
//            string filePath = Path.Combine(folderPath, uniqueFileName);

//            Directory.CreateDirectory(folderPath);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            return Path.Combine("/", Path.Combine(subfolders), uniqueFileName).Replace("\\", "/");
//        }

//        public async Task<string> UploadAsync(IFormFile formFile)
//        {
//            var imageDirectory = Path.Combine(_environment.WebRootPath, "assets", "img");
//            var tempDirectory = Path.Combine(_environment.WebRootPath, "assets", "temp");

//            // Ensure the image and temp directories exist
//            if (!Directory.Exists(imageDirectory))
//                Directory.CreateDirectory(imageDirectory);

//            if (!Directory.Exists(tempDirectory))
//                Directory.CreateDirectory(tempDirectory);

//            // Get the existing image files to calculate the new number
//            var existingFiles = Directory.GetFiles(imageDirectory, "image-*.png")
//                .Select(p => Path.GetFileNameWithoutExtension(p))
//                .Select(p => p.Replace("image-", ""))
//                .Where(p => int.TryParse(p, out _))
//                .Select(int.Parse)
//                .ToList();

//            // Yeni numara belirle
//            int newNumber = existingFiles.Any() ? existingFiles.Max() + 1 : 1;


//            var fileName = $"image-{newNumber}.png";
//            var tempPath = Path.Combine(tempDirectory, fileName);
//            var imagePath = Path.Combine(imageDirectory, fileName);

//            // Save the file to the temp directory
//            using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
//            {
//                await formFile.CopyToAsync(fileStream);
//            }

//            // Process the image (e.g., resizing, encoding)
//            using (var image = await Image.LoadAsync(tempPath))
//            {
//                await image.SaveAsync(imagePath, new PngEncoder());
//            }
//            // Clean up the temporary file
//            File.Delete(tempPath);

//            return fileName;
//        }

//    }
//}
