using Microsoft.AspNetCore.Http;

namespace Business.Extensions
{
    public static class FileExtensions
    {
        public static async Task<string> SaveFilesAsync(this IFormFile file, string root, string client, string folderNameAssets, string folderNameImages, string folderName)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullPath = Path.Combine(root, "wwwroot", client, folderNameAssets, folderNameImages, folderName, uniqueFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            using FileStream fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);
            return Path.Combine(client, folderNameAssets, folderNameImages, folderName, uniqueFileName).Replace("\\", "/"); // for returning relative web path
        }


        public static bool CheckFileType(this IFormFile file, string fileType)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.ContentType.StartsWith(fileType))
            {
                return true;
            }
            return false;
        }

        public static bool CheckFileSize(this IFormFile file, int fileSize)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length < fileSize * 1024 * 1024)
            {
                return true;
            }
            return false;
        }

        public static void DeleteFile(string root, string client, string folderNameAssets, string folderNameImages, string folderName, string fileName)
        {
            string path = Path.Combine(root, "wwwroot", client, folderNameAssets, folderNameImages, folderName, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }


    }
}
