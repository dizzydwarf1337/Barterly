using Application.DTOs;
using Application.Features.Users.Events.UserCreated;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, ILogService logService, IMapper mapper, IMediator mediator, IConfiguration config)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logService = logService;
            _mapper = mapper;
            _mediator = mediator;
            _config = config;
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isPasswordCorrect)
            {
                throw new ArgumentException("Invalid password");
            }
            if (!isEmailConfirmed)
            {
                throw new UnauthorizedAccessException("Confirm email first");
            }
            user.LastSeen = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            await _logService.CreateLogAsync("User logged in", LogType.Information, userId: user.Id);
            var token = await _tokenService.GetLoginToken(user.Id);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.token = token;
            userDto.Role = await _userManager.IsInRoleAsync(user, "Admin") ? "Admin" : (await _userManager.IsInRoleAsync(user, "Moderator") ? "Moderator" : "User");
            return userDto;
        }

        public Task LoginWithFaceBook()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> LoginWithGmail(string code)
        {
            TokenResponse token = null;
            try
            {
                token = await GetGoogleAccessTokenAsync(code);
                if (token == null)
                {
                    throw new Exception("Failed to retrieve access token from Google.");
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message +ex.StackTrace);
            }



            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _config["GoogleAPI:ClientId"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken, settings);
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if(user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    ProfilePicturePath = payload.Picture,
                    UserName=payload.Email,
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(user, "TemporaryPassword123!");
                if (result.Succeeded)
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddToRoleAsync(user, "User");
                }
            }
            var tokenToSend = await _tokenService.GetLoginToken(user.Id);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.token = tokenToSend;
            userDto.Role = "User";
            return userDto;
        }
        public async Task LogOut(string token)
        {
            await _tokenService.DeleteToken(token);
            await _logService.CreateLogAsync("User logged out", LogType.Information);
        }

        public async Task Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            if (user == null) throw new ArgumentNullException(nameof(user), "User is null before hashing password.");
            if (_userManager == null) throw new InvalidOperationException("UserManager is not initialized.");

            user.UserName = user.Email;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerDto.Password);
            try
            {
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    await _logService.CreateLogAsync($"User registration failed for {user.Email}. Errors: {errors}",
                        LogType.Error);
                    throw new Exception(errors);
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error while saving user: {ex.InnerException?.Message ?? ex.Message}");
            }
            await _userManager.AddToRoleAsync(user, "User");

            var userCreatedEvent = new UserCreatedEvent(user.Email, user.FirstName, user.LastName,user.Id);
            await _mediator.Publish(userCreatedEvent);

            await _logService.CreateLogAsync($"User created successfully: {user.Email}",LogType.Information,userId: user.Id);
        }

        private async Task<TokenResponse> GetGoogleAccessTokenAsync(string code)
        {
            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _config["GoogleAPI:ClientId"] },          
                { "client_secret", _config["GoogleAPI:Key"] },  
                { "redirect_uri", _config["GoogleAPI:RedirectUri"] },    
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                return tokenResponse;
            }

            return null; 
        }
    }
}
