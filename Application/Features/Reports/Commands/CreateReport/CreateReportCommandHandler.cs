using API.Core.ApiResponse;
using Application.Core.Factories.Interfaces;
using Domain.Entities.Posts;
using Domain.Entities.Users;
using Domain.Exceptions.BusinessExceptions;
using Domain.Interfaces.Commands.Post;
using Domain.Interfaces.Commands.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.CreateReport
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ApiResponse<Unit>>
    {
        private readonly IReportFactory _reportFactory;

        public CreateReportCommandHandler(IReportFactory reportFactory)
        {
            _reportFactory = reportFactory;
        }

        public async Task<ApiResponse<Unit>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            await _reportFactory.CreateReportAsync(request.createReportDto);
            return ApiResponse<Unit>.Success(Unit.Value, 201);
        }
    }
}
