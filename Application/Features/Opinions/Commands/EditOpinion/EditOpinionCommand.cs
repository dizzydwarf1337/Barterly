using API.Core.ApiResponse;
using Application.DTOs.General.Opinions;
using Application.Interfaces.CommandInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.EditOpinion
{
    public class EditOpinionCommand : IRequest<ApiResponse<OpinionDto>>, IHasOwner, IOpinionOwner, IMessageContainer
    {
        public required EditOpinionDto EditOpinionDto { get; set; }


        public string Message { get => EditOpinionDto.Content; set => EditOpinionDto.Content=value; }

        public string OpinionId => EditOpinionDto.Id;

        public string OwnerId => EditOpinionDto.AuthorId;
    }
}
