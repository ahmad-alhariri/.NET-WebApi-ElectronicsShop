using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Application.Interfaces.Services;

public interface IFileService
{
    /// <summary>
    ///     Saves a single image file and returns the relative URL.
    /// </summary>
    Task<string> SaveImageAsync(IFormFile imageFile, string folderName);

    /// <summary>
    ///     Saves multiple image files and returns their relative URLs.
    /// </summary>
    Task<IEnumerable<string>> SaveImagesAsync(IEnumerable<IFormFile> imageFiles, string folderName);

    Task<bool> DeleteImageAsync(string imagePath);
}