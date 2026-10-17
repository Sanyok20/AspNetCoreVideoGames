using Microsoft.Extensions.FileProviders;

namespace VideoGames.Infrastructure
{
    public static class ConfigureStaticFiles
    {
        public static void AddStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            string storagePath = Path.Combine(env.ContentRootPath, "FileStorage");

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            string imagesPath = Path.Combine(storagePath, "images");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/images",
                FileProvider = new PhysicalFileProvider(imagesPath)
            });
        }
    }
}