using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly ILogService _logService;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly ITokenCommandRepository _tokenCommandRepository;
    private readonly ITokenQueryRepository _tokenQueryRepository;
    private readonly UserManager<User> _userManager;

    private readonly byte[] _key;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(
        ILogService logService,
        UserManager<User> userManager,
        IConfiguration configuration,
        ITokenQueryRepository tokenQueryRepository,
        ITokenCommandRepository tokenCommandRepository)
    {
        _logService = logService;
        _tokenCommandRepository = tokenCommandRepository;
        _tokenQueryRepository = tokenQueryRepository;
        _userManager = userManager;
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");

        _key = Encoding.UTF8.GetBytes(_jwtSettings["Key"] ?? throw new ConfigException("JWT Key is missing"));
        _issuer = _jwtSettings["Issuer"] ?? throw new ConfigException("JWT Issuer is missing");
        _audience = _jwtSettings["Audience"] ?? throw new ConfigException("JWT Audience is missing");
    }

    private string CreateJwtToken(Guid userId, IEnumerable<Claim> claims)
    {
        var symmetricKey = new SymmetricSecurityKey(_key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(4),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task DeleteTokenByUserMail(string userMail, TokenType tokenType)
    {
        var userId = (await _userManager.FindByEmailAsync(userMail) ?? throw new Exception("Cannot find user by email, while checking token")).Id;
        await _tokenCommandRepository.DeleteToken(userId, tokenType);
        await _logService.CreateLogAsync($"Token with provided type {tokenType} was deleted or expired", LogType.Information, userId: userId);
    }
    public async Task DeleteToken(string token)
    {
        await _tokenCommandRepository.DeleteToken(token);
    }

    private async Task<string> GenerateAuthToken(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new InvalidDataProvidedException("Wrong user Id");
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = CreateJwtToken(userId, claims);
        await _tokenCommandRepository.AddToken(userId, token, "Bearer", TokenType.Auth);
        await _logService.CreateLogAsync("Generated auth token", LogType.Information, null, Guid.Empty, userId);
        return token;
    }

    public async Task<string> GenerateEmailConfirmationToken(string userMail)
    {
        var user = await _userManager.FindByEmailAsync(userMail) ?? throw new EntityNotFoundException("User");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _tokenCommandRepository.AddToken(user.Id, token, "user manager", TokenType.EmailConfirmation);
        await _logService.CreateLogAsync("Generated email confirmation token", LogType.Information, null, Guid.Empty, user.Id);
        return token;
    }

    public async Task<string> GeneratePasswordResetToken(string userMail)
    {
        var user = await _userManager.FindByIdAsync(userMail) ?? throw new EntityNotFoundException("User");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _tokenCommandRepository.AddToken(user.Id, token, "user manager", TokenType.PasswordReset);
        return token;
    }

    public async Task<bool> CheckUserToken(string userMail, TokenType tokenType, string token)
    {
        var userId = (await _userManager.FindByEmailAsync(userMail) ?? throw new EntityNotFoundException("User")).Id;
        var dbToken = await _tokenQueryRepository.GetTokenByUserIdAsync(userId, tokenType);
        if (dbToken != null && dbToken.Value == token)
        {
            await _logService.CreateLogAsync("Token confirmed successfully", LogType.Information, null, Guid.Empty, userId);
            return true;
        }
        await _logService.CreateLogAsync("Token is invalid", LogType.Warning, null, Guid.Empty, userId);
        return false;
    }

    public async Task<string> GetLoginToken(Guid userId)
    {
        string token;
        try
        {
            token = (await _tokenQueryRepository.GetTokenByUserIdAsync(userId, TokenType.Auth)).Value;
        }
        catch (Exception)
        {
            token = await GenerateAuthToken(userId);
        }
        return token;
    }
}
