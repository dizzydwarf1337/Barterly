using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Microsoft.AspNetCore.Identity;

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
            var user = await _userManager.FindByEmailAsync(email) ?? throw new EntityNotFoundException($"User with email {email}");

            var result = await _tokenService.CheckUserToken(email, TokenType.EmailConfirmation, token);

            if (!result)
            {
                throw new AccessForbiddenException("UserService.ConfirmEmail",$"{user.Id}","Email token is invalid or expired");
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
            {
                throw new InvalidDataProvidedException("Email confirmation failed");
            }
            await _tokenService.DeleteTokenByUserMail(user.Email, TokenType.EmailConfirmation);
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
                throw new EntityNotFoundException("User not found");
            }
            var IsCorrect = await _tokenService.CheckUserToken(email, TokenType.PasswordReset, token);
            if (!IsCorrect)
            {
                throw new AccessForbiddenException("UserService",user.Id.ToString(),"Reset password token is invalid");
            }

            var res = await _userManager.ResetPasswordAsync(user, token, password);
            if (res.Succeeded)
            {
                await _tokenService.DeleteTokenByUserMail(email, TokenType.PasswordReset);
            }
            else throw new AccessForbiddenException("UserService.ResetPassword",user.Id.ToString(),"User manager can't confirm reset token");
        }

        public Task UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
