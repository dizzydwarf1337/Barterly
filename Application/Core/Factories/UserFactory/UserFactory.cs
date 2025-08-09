using Application.Core.Factories.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enums.Users;
using Domain.Exceptions.BusinessExceptions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.Factories.UserFactory;

public class UserFactory : IUserFactory
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserFactory(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<User> CreateUser(string FirstName, string LastName, string Email, string Password,
        UserRoles Role)
    {
        var user = new User { FirstName = FirstName, LastName = LastName, Email = Email };
        user.UserName = user.Email;
        user.NormalizedUserName = user.Email!.ToUpper();


        var userSettings = new UserSettings { UserId = user.Id, User = user };
        var userActivity = new UserActivitySummary { UserId = user.Id, User = user };

        user.UserActivitySummary = userActivity;
        user.UserActivitySummaryId = userActivity.Id;
        user.Setting = userSettings;
        user.UserSettingId = userSettings.Id;

        var result = await _userManager.CreateAsync(user, Password);

        if (!result.Succeeded)
            throw new EntityCreatingException("User", string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user,
            Role == UserRoles.Admin ? "Admin" : Role == UserRoles.Moderator ? "Moderator" : "User");

        return user;
    }

    public async Task<User> CreateUser(GoogleJsonWebSignature.Payload payload)
    {
        var user = new User
        {
            Email = payload.Email,
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            ProfilePicturePath = payload.Picture,
            UserName = payload.Email,
            EmailConfirmed = true
        };
        var userSettings = new UserSettings { UserId = user.Id, User = user };
        var userActivity = new UserActivitySummary { UserId = user.Id, User = user };

        user.UserActivitySummary = userActivity;
        user.UserActivitySummaryId = userActivity.Id;
        user.Setting = userSettings;
        user.UserSettingId = userSettings.Id;

        var result = await _userManager.CreateAsync(user, "TemporaryPassword123!");
        if (result.Succeeded)
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            throw new EntityCreatingException("User", string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user;
    }
}