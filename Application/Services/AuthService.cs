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
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, ITokenService tokenService, IMapper mapper, IConfiguration config, IUserFactory userFactory)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _config = config;
            _userFactory = userFactory;
        }

        public Task LoginWithFaceBook()
        {
            throw new NotImplementedException();
        }

        public async Task<LoginExternalResponse> LoginWithGmail(string code)
        {
            TokenResponse token = await GetGoogleAccessTokenAsync(code) ?? throw new ExternalServiceException("Failed to retrive google token");
            bool IsFirstTime = false;
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _config["GoogleAPI:ClientId"] ?? throw new ConfigException("Google audience error") }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken, settings);
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                IsFirstTime = true;
                user = await _userFactory.CreateUser(payload);
            }
            var tokenToSend = await _tokenService.GetLoginToken(user.Id);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.token = tokenToSend;
            userDto.Role = "User";
            return new LoginExternalResponse { UserDto = userDto, IsFirstTime = IsFirstTime};
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
