using Application.DTOs.General.Opinions;
using Application.DTOs.User;
using Application.Features.Users.Commands.ConfirmEmail;
using Application.Features.Users.Commands.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        public UserController() : base() { }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            return HandleResponse(await Mediator.Send(new ConfirmEmailCommand { userMail = confirmEmailDto.Email, token = confirmEmailDto.Token }));
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            return HandleResponse(await Mediator.Send(new ResetPasswordCommand { Email = resetPasswordDto.Email, Password = resetPasswordDto.Password, Token = resetPasswordDto.Token }));
        }

    }
}
