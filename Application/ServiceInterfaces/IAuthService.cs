using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto loginDto);
        Task LoginWithGmail();
        Task LoginWithFaceBook();
        Task LogOut(Guid userId);
    }
}
