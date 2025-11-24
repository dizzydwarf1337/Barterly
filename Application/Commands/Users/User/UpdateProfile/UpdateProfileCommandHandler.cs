using Application.Core.ApiResponse;
using Application.Interfaces;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users.User.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ApiResponse<UpdateProfileCommand.Result>>
{
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IFileService _fileService;

    public UpdateProfileCommandHandler(IUserCommandRepository userCommandRepository,
        IUserQueryRepository userQueryRepository,
        IFileService fileService)
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
        _fileService = fileService;
    }
    
    public async Task<ApiResponse<UpdateProfileCommand.Result>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUsers()
            .Where(x => x.Id == request.AuthorizeData!.UserId && !(x.Setting!.IsBanned || x.Setting.IsDeleted))
            .FirstOrDefaultAsync(cancellationToken);
        if(user == null)
            return ApiResponse<UpdateProfileCommand.Result>.Failure("User doesnt exist or restricted");
        user.Bio = request.Bio;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Country = request.Country;
        user.City = request.City;
        user.Street = request.Street;
        user.HouseNumber = request.HouseNumber;
        user.PostalCode = request.PostalCode;
        user.ProfilePicturePath = request.ProfilePicturePath == null && request.File != null ? await _fileService.SaveFile(request.File) : request.ProfilePicturePath;
        await _userCommandRepository.UpdateUserAsync(user, cancellationToken);
        return  ApiResponse<UpdateProfileCommand.Result>.Success(new UpdateProfileCommand.Result
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Bio = user.Bio,
            Country = user.Country,
            City = user.City,
            Street = user.Street,
            HouseNumber = user.HouseNumber,
            PostalCode = user.PostalCode,
            ProfilePicturePath = user.ProfilePicturePath,
        });
    }
}