using Application.DTOs;
using Application.ServiceInterfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService : ITokenService
{
    private readonly ILogService _logService;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly ITokenCommandRepository _tokenCommandRepository;
    private readonly ITokenQueryRepository _tokenQueryRepository;
    private readonly UserManager<User> _userMenager;

    private readonly byte[] _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenService(
        ILogService logService,
        UserManager<User> userMenager,
        IConfiguration configuration,
        ITokenQueryRepository tokenQueryRepository,
        ITokenCommandRepository tokenCommandRepository)
    {
        _logService = logService;
        _tokenCommandRepository = tokenCommandRepository;
        _tokenQueryRepository = tokenQueryRepository;
        _userMenager = userMenager;
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");

        _key = Encoding.UTF8.GetBytes(_jwtSettings["Key"] ?? throw new Exception("JWT Key is missing"));
        _issuer = _jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer is missing");
        _audience = _jwtSettings["Audience"] ?? throw new Exception("JWT Audience is missing");
    }

    private string CreateJwtToken(Guid userId, IEnumerable<Claim> claims)
    {
        var symmetricKey = new SymmetricSecurityKey(_key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task DeleteTokenByUserId(Guid userId, string tokenType)
    {
        await _tokenCommandRepository.DeleteToken(userId, tokenType);
        await _logService.CreateLogAsync( $"Token with provided type {tokenType} was deleted or expired", LogType.Information, null, userId, Guid.Empty);
    }

    public async Task<string> GenerateAuthToken(Guid userId)
    {
        var user = await _userMenager.FindByIdAsync(userId.ToString()) ?? throw new Exception("Wrong user Id");
        var roles = await _userMenager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = CreateJwtToken(userId, claims);
        await _tokenCommandRepository.AddToken(userId, token, "Bearer", "auth");
        await _logService.CreateLogAsync("Generated auth token", LogType.Information,null,Guid.Empty, userId);
        return token;
    }

    public async Task<string> GenerateEmailConfirmationToken(Guid userId)
    {
        var user = await _userMenager.FindByIdAsync(userId.ToString()) ?? throw new Exception("User not found");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var token = CreateJwtToken(userId, claims);
        await _tokenCommandRepository.AddToken(userId, token, "Bearer", "email");
        await _logService.CreateLogAsync("Generated email confirmation token", LogType.Information,null, Guid.Empty, userId); 
        return token;
    }

    public async Task<bool> CheckUserToken(Guid userId, string tokenType, string token)
    {
        if (await _tokenQueryRepository.GetTokenByUserIdAsync(userId, tokenType) != null)
        {
            await _logService.CreateLogAsync("Token confirmed successfully", LogType.Warning, null, Guid.Empty, userId);
            return true;
        }
        await _logService.CreateLogAsync("Token is invalid", LogType.Warning, null, Guid.Empty, userId);
        return false;
    }
}
