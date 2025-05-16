using Application.Core.Factories.Interfaces;
using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Features.Users.Events.UserCreated;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Domain.Exceptions.ExternalServicesExceptions;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserFactory _userFactory;
        private readonly ITokenService _tokenService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, ILogService logService, IMapper mapper, IConfiguration config, IUserFactory userFactory)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logService = logService;
            _mapper = mapper;
            _config = config;
            _userFactory = userFactory;
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new EntityNotFoundException("User");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isPasswordCorrect)
            {
                throw new InvalidDataProvidedException("Password","User","AuthService.Login");
            }
            if (!isEmailConfirmed)
            {
                throw new AccessForbiddenException("AuthService.Login",user.Id.ToString(),"Confirm email first");
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
            TokenResponse token = await GetGoogleAccessTokenAsync(code) ?? throw new ExternalServiceException("Failed to retrive google token");
              
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _config["GoogleAPI:ClientId"] ?? throw new ConfigException("Google audience error") }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken, settings);
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    ProfilePicturePath = payload.Picture,
                    UserName = payload.Email,
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
            var user = await _userFactory.CreateUser(registerDto);
            if (user ==null)
            {
                await _logService.CreateLogAsync($"User registration failed for {registerDto.Email}.",LogType.Error);
                throw new EntityCreatingException("User","AuthService.Register");
            }
        }

        private async Task<TokenResponse> GetGoogleAccessTokenAsync(string code)
        {
            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _config["GoogleAPI:ClientId"] ?? throw new ConfigException("Google client id error")},
                { "client_secret", _config["GoogleAPI:Key"] ?? throw new ConfigException("Google client secret error") } ,
                { "redirect_uri", _config["GoogleAPI:RedirectUri"] ?? throw new ConfigException("Google Redirect api error") },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent) ?? throw new ExternalServiceException("Google token null response");
                return tokenResponse;
            }
            else throw new ExternalServiceException("Failed to get google access token");
        }
    }
}
