using Application.Interfaces;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class FileService : IFileService
{
    private readonly string _baseFilePath = "wwwroot/";

    public async Task<string> SaveFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new InvalidDataProvidedException("File", "", "FileService.SaveFile");

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_baseFilePath, "uploads", fileName);

        Directory.CreateDirectory(_baseFilePath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("uploads", fileName).Replace("\\", "/");
    }

    public Task DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) throw new EntityNotFoundException("File path");

        var fullPath = Path.Combine(_baseFilePath, filePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
        else
            throw new EntityNotFoundException("File");

        return Task.CompletedTask;
    }
}