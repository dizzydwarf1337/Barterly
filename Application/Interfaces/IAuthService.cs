using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task<UserDto> LoginWithGmail(string token);
        Task Register(RegisterDto registerDto);
        Task LoginWithFaceBook();
        Task LogOut(string userMail);
    }
}
