using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FileService : IFileService
    {
        public Task DeleteFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
