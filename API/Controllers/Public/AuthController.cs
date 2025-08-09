using Application.Commands.Public.Accounts.CreateUser;
using Application.Commands.Public.Accounts.ResendEmailConfirm;
using Application.Commands.Public.Auth.Login;
using Application.Commands.Public.Auth.LoginWithGoogle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Public;

[Route("public/auth")]
[AllowAnonymous]
public class AuthController : BaseController
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(CreateUserCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("login-google")]
    public async Task<IActionResult> LoginWithGoogle(LoginWithGoogleCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }

    [HttpPost]
    [Route("resend-email-confirm")]
    public async Task<IActionResult> ResendEmailConfirm(ResendEmailConfirmCommand command)
    {
        return HandleResponse(await Mediator.Send(command));
    }
}