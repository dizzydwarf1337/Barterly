using Application.Core.Factories.Interfaces;
using Application.DTOs.Auth;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Factories.UserFactory
{
    public class UserFactory : IUserFactory
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UserFactory(UserManager<User> userManager,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<User> CreateUser(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            user.UserName = user.Email;
            user.NormalizedUserName = user.Email.ToUpper();


            var userSettings = new UserSettings { UserId = user.Id, User = user };
            var userActivity = new UserActivitySummary { UserId = user.Id, User = user };

            user.UserActivitySummary = userActivity;
            user.UserActivitySummaryId = userActivity.Id;
            user.Setting = userSettings;
            user.UserSettingId = userSettings.Id;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                throw new EntityCreatingException("User", string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

            return user;
        }
    }
}
