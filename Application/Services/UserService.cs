using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        public UserService(ITokenService tokenService, UserManager<User> userManager, IUserQueryRepository userQueryRepository, IUserCommandRepository userCommandRepository, IMapper mapper, ILogService logService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _userQueryRepository = userQueryRepository;
            _userCommandRepository = userCommandRepository;
            _mapper = mapper;
            _logService = logService;
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
            await _logService.CreateLogAsync($"User {user.Email} confirmed email", LogType.Information, userId: user.Id);
            await _tokenService.DeleteTokenByUserMail(user.Email, TokenType.EmailConfirmation);
        }

        public Task DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            return _mapper.Map<UserDto>(await _userQueryRepository.GetUserAsync(userId));
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
        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userQueryRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            return _mapper.Map<UserDto>(user);
        }
        public Task UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
