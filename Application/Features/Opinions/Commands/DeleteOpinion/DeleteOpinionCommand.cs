using API.Core.ApiResponse;
using Application.DTOs.General;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.DeleteOpinion
{
    public class DeleteOpinionCommand : IRequest<ApiResponse<Unit>>, IOpinionOwner
    {
        public required string OpinionId { get; set; }

    }
}
