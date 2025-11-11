using Application.Core.ApiResponse;

namespace Application.Core.MediatR.Requests;

public class ModeratorRequest<T> : AuthorizedRequest<ApiResponse<T>>
{
}