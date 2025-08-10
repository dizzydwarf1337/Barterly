using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities.Users;
using Domain.Enums.Common;
using Domain.Exceptions.BusinessExceptions;
using Domain.Exceptions.DataExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly string _audience;
    private readonly IConfiguration _configuration;
    private readonly string _issuer;
    private readonly IConfigurationSection _jwtSettings;

    private readonly byte[] _key;
    private readonly ILogService _logService;
    private readonly UserManager<User> _userManager;

    public TokenService(
        ILogService logService,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _logService = logService;
        _userManager = userManager;
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");

        _key = Encoding.UTF8.GetBytes(_jwtSettings["Key"] ?? throw new ConfigException("JWT Key is missing"));
        _issuer = _jwtSettings["Issuer"] ?? throw new ConfigException("JWT Issuer is missing");
        _audience = _jwtSettings["Audience"] ?? throw new ConfigException("JWT Audience is missing");
    }

    public async Task DeleteAuthToken(string userMail)
    {
        var user = await _userManager.FindByEmailAsync(userMail) ?? throw new EntityNotFoundException("User");
        await _userManager.RemoveAuthenticationTokenAsync(user, "App", "JWT");
    }

    public async Task<string> GenerateEmailConfirmationToken(string userMail, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(userMail) ?? throw new EntityNotFoundException("User");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _logService.CreateLogAsync("Generated email confirmation token", cancellationToken, LogType.Information,
            null, Guid.Empty, user.Id);
        return token;
    }

    public async Task<string> GeneratePasswordResetToken(string userMail)
    {
        var user = await _userManager.FindByIdAsync(userMail) ?? throw new EntityNotFoundException("User");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task<bool> CheckUserToken(string userMail, TokenType tokenType, string token,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(userMail) ?? throw new EntityNotFoundException("User");
        var result = false;
        switch (tokenType)
        {
            case TokenType.Auth:
                result = await _userManager.VerifyUserTokenAsync(user, "App", " ", token);
                break;
            case TokenType.EmailConfirmation:
                result = await _userManager.VerifyUserTokenAsync(user, "App", "EmailConfirmation", token);
                break;
            case TokenType.PasswordReset:
                result = await _userManager.VerifyUserTokenAsync(user, "App", "PasswordReset", token);
                break;
        }

        await _logService.CreateLogAsync("Token is invalid", cancellationToken, LogType.Warning, null, Guid.Empty,
            user.Id);
        return result;
    }

    public async Task<string> GetLoginToken(Guid userId, CancellationToken cancToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId) ??
                   throw new EntityNotFoundException("User");

        var token = await _userManager.GetAuthenticationTokenAsync(user, "App", "JWT");
        if (string.IsNullOrEmpty(token)) token = await GenerateAuthToken(userId, cancToken);
        return token;
    }

    private string CreateJwtToken(Guid userId, IEnumerable<Claim> claims)
    {
        var symmetricKey = new SymmetricSecurityKey(_key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.UtcNow.AddDays(4),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GenerateAuthToken(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                   throw new InvalidDataProvidedException("Wrong user Id");
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = CreateJwtToken(userId, claims);
        await _userManager.SetAuthenticationTokenAsync(user, "App", "JWT", token);
        await _logService.CreateLogAsync("Generated auth token", cancellationToken, LogType.Information, null,
            Guid.Empty, userId);
        return token;
    }
}