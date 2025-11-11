using Application.Core.ApiResponse;
using Application.Interfaces;
using Domain.Interfaces.Queries.User;
using MediatR;

namespace Application.Commands.Public.Auth.LoginWithGoogle;

public class
    LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommand,
    ApiResponse<LoginWithGoogleCommand.Result>>
{
    private readonly IAuthService _authService;
    private readonly IMailService _mailService;
    private readonly IUserFavPostQueryRepository _favPostQueryRepository;
    private readonly INotificationQueryRepository _notificationQueryRepository;

    public LoginWithGoogleCommandHandler(IAuthService authService, IMailService mailService,
        IUserFavPostQueryRepository favPostQueryRepository,
        INotificationQueryRepository notificationQueryRepository)
    {
        _authService = authService;
        _mailService = mailService;
        _favPostQueryRepository = favPostQueryRepository;
        _notificationQueryRepository = notificationQueryRepository;
    }

    public async Task<ApiResponse<LoginWithGoogleCommand.Result>> Handle(LoginWithGoogleCommand request,
        CancellationToken cancellationToken)
    {
        var loginResponse = await _authService.LoginWithGmail(request.token, cancellationToken);
        if (loginResponse.IsFirstTime)
            await _mailService.SendMail(loginResponse.UserDto.Email, "External login",
                $@"
                <body>
                <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                        <h2 style=""color: #2c3e50; "">Welcome, {loginResponse.UserDto.FirstName} {loginResponse.UserDto.LastName}!</h2>
                        <p style=""font-size: 16px; line-height: 1.6;"">
                            You have successfully logged in to our platform using your Google account.
                        </p>

                        <div style='text-align: center; margin: 30px 0;'>
                                <a href=""https://localhost:3000"" style="" background-color: #008b8b;color: white; padding: 12px 25px; text-decoration: none; border-radius: 4px; font-size: 16px;"">
                                Go to Website
                                </a>
                        </div>
                        <p style='font-size: 12px; color: #777;'>
                            If this was you, you can continue enjoying all the features of our service. If you did not initiate this login, please contact our support team immediately.
                        </p>
                        <p style=""font-size: 14px; color: #777; margin-top: 30px;"">
                            — The Barterly Team
                        </p>
                    </div>
                </div>
                </body>"
            );

        var favPostsCount = (await _favPostQueryRepository.GetUserFavPostsByUserIdAsync(Guid.Parse(loginResponse.UserDto.Id), cancellationToken)).Count;
        var notificationCount = (await _notificationQueryRepository.GetNotificationsByUserIdAsync(Guid.Parse(loginResponse.UserDto.Id),cancellationToken)).Count();
        return ApiResponse<LoginWithGoogleCommand.Result>.Success(
            new LoginWithGoogleCommand.Result
            {
                Email = loginResponse.UserDto.Email,
                FirstName = loginResponse.UserDto.FirstName,
                LastName = loginResponse.UserDto.LastName,
                Id = loginResponse.UserDto.Id,
                ProfilePicturePath = loginResponse.UserDto.ProfilePicturePath,
                Role = loginResponse.UserDto.Role,
                token = loginResponse.UserDto.token,
                FavPostsCount = favPostsCount,
                NotificationCount = notificationCount,
            });
    }
}