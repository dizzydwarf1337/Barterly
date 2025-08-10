using Application.Core.ApiResponse;
using Application.Interfaces;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Public.Accounts.ResendEmailConfirm;

public class ResendEmailConfirmCommandHandler : IRequestHandler<ResendEmailConfirmCommand, ApiResponse<Unit>>
{
    private readonly IMailService _mailService;
    private readonly UserManager<User> _userManager;
    public ResendEmailConfirmCommandHandler(IMailService mailService)
    {
        _mailService = mailService;
    }

    public async Task<ApiResponse<Unit>> Handle(ResendEmailConfirmCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if(user == null) 
            return ApiResponse<Unit>.Failure("Użytkownik nie istnieje.");
        
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
            return ApiResponse<Unit>.Failure("Adres email został już podtwierdzony");
        
        await _mailService.SendConfiramationMail(request.Email, cancellationToken);
        return ApiResponse<Unit>.Success(Unit.Value);
    }
}