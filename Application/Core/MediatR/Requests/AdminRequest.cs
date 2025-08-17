using Application.Core.ApiResponse;

namespace Application.Core.MediatR.Requests;

public class AdminRequest<T> : AuthorizedRequest<ApiResponse<T>>
{
}