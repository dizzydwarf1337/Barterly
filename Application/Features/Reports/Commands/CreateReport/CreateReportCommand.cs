using API.Core.ApiResponse;
using Application.DTOs.General;
using Application.DTOs.Reports;
using Application.Interfaces.CommandInterfaces;
using MediatR;

namespace Application.Features.Reports.Commands.CreateReport
{
    public class CreateReportCommand : IRequest<ApiResponse<Unit>>, IHasOwner
    {
        public required CreateReportDto createReportDto { get; set; }

        public string OwnerId => createReportDto.AuthorId;
    }
}
