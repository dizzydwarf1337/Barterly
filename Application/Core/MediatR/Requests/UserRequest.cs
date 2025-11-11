using Application.Core.ApiResponse;

namespace Application.Core.MediatR.Requests;

public class UserRequest<T> : AuthorizedRequest<ApiResponse<T>>
{
}