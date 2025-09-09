using ElectronicsShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Infrastructure.Services;

public class FileService:IFileService
{
    private readonly IHostingEnvironment _environment;

    public FileService(IHostingEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName)
    {
        if (imageFile == null || imageFile.Length == 0)
            return "Image file is required";

        var uploadsFolder = Path.Combine(_environment.WebRootPath, folderName);

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return Path.Combine(folderName, uniqueFileName).Replace("\\", "/"); // return relative path
    }

    public async Task<IEnumerable<string>> SaveImagesAsync(IEnumerable<IFormFile> imageFiles, string folderName)
    {
        var tasks = imageFiles.Select(image => SaveImageAsync(image, folderName)).ToList();
        return (await Task.WhenAll(tasks)).Where(url => url != null).ToList()!;

    }

    public Task<bool> DeleteImageAsync(string imagePath)
    {
        var fullPath = Path.Combine(_environment.WebRootPath,
            imagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}