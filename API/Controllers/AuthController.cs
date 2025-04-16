using Application.DTOs;
using Application.DTOs.Auth;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.LoginWithGoogle;
using Application.Features.Users.Commands.Logout;
using Application.Features.Users.Commands.ResendEmailConfirm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController() : base()
        {

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            return HandleResponse(await Mediator.Send(new CreateUserCommand { registerDto = register }));

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return HandleResponse(await Mediator.Send(new LoginCommand { loginDto = loginDto }));
        }
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginDto googleLoginDto)
        {
            Console.WriteLine(googleLoginDto.token);
            return HandleResponse(await Mediator.Send(new LoginWithGoogleCommand { googleLoginDto = googleLoginDto }));
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return HandleResponse(await Mediator.Send(new LogoutCommand { token = token }));
        }

        [HttpPost("resendEmailConfirm")]
        public async Task<IActionResult> ResendEmailConfirm(EmailDto email)
        {
            return HandleResponse(await Mediator.Send(new ResendEmailConfirmCommand { Email = email.Email }));
        }
    }
}
