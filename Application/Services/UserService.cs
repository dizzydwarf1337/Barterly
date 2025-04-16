using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;



        public UserService(ITokenService tokenService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} not found.");
            }
            
            var result = await _tokenService.CheckUserToken(email, Domain.Enums.TokenType.EmailConfirmation, token);
            
            if (!result)
            {
                throw new Exception("Email token is invalid or expired");
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
            {
                throw new Exception("Email confirmation failed");
            }
             await _tokenService.DeleteTokenByUserMail(user.Email, Domain.Enums.TokenType.EmailConfirmation);
        }

        public Task DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task ResetPassword(string email, string token, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var IsCorrect = await _tokenService.CheckUserToken(email, TokenType.PasswordReset, token);
            if (!IsCorrect)
            {
                throw new Exception("Reset password token is invalid");    
            }
            try
            {
                var res = await _userManager.ResetPasswordAsync(user, token, password);
                if (res.Succeeded)
                {
                    await _tokenService.DeleteTokenByUserMail(email, TokenType.PasswordReset);
                }
            }
            catch
            {
                throw new Exception("User manager can't confirm reset token");
            }
        }

        public Task UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
