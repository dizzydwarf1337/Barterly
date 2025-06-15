using API.Core.ApiResponse;
using Application.DTOs.General.Opinions;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.CreateOpinion
{
    public class CreateOpinionCommand : IRequest<ApiResponse<OpinionDto>>, IHasOwner, IMessageContainer
    {
        public required CreateOpinionDto createOpinionDto { get; set; }
        public string OwnerId => createOpinionDto.AuthorId;

        public string Message { get => createOpinionDto.Content; set => createOpinionDto.Content=value; }
    }
}
