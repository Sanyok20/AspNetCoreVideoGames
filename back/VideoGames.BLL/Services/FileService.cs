using Microsoft.AspNetCore.Http;

namespace VideoGames.BLL.Services
{
    public class FileService
    {
        public async Task<string?> SaveImageAsync(IFormFile file, string folderPath, CancellationToken ct = default)
        {
            var types = file.ContentType.Split("/");

            if (types.Length != 2 || types[0] != "image")
            {
                return null;
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var ext = Path.GetExtension(file.FileName);
            var imageName = $"{Guid.NewGuid()}{ext}";
            var imagePath = Path.Combine(folderPath, imageName);

            using var stream = File.Create(imagePath);
            await file.CopyToAsync(stream, ct);

            return imageName;
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}