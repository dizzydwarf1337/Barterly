using Application.DTOs;
using Application.ServiceInterfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogService _logService;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, ILogService logService) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logService = logService;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                throw new Exception("Invalid password");
            }
            await _logService.CreateLogAsync("User logged in", LogType.Information,null, null, user.Id);
            return await _tokenService.GenerateAuthToken(user.Id);
        }

        public Task LoginWithFaceBook()
        {
            throw new NotImplementedException();
        }

        public Task LoginWithGmail()
        {
            throw new NotImplementedException();
        }
        public async Task LogOut(Guid userId)
        {
            await _tokenService.DeleteTokenByUserId(userId, "auth");
            await _logService.CreateLogAsync("User logged out", LogType.Information, null, null, userId);
        }
    }
}
