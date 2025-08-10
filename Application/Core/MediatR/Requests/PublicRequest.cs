using Application.Core.ApiResponse;
using MediatR;

namespace Application.Core.MediatR.Requests;

public class PublicRequest<T> : IRequest<ApiResponse<T>>
{
}