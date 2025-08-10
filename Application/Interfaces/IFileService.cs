using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileService
{
    Task<string> SaveFile(IFormFile file);
    Task DeleteFile(string FilePath);
}