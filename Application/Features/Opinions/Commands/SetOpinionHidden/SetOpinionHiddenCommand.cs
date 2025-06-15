using API.Core.ApiResponse;
using Application.DTOs.General.Opinions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Opinions.Commands.SetOpinionHidden
{
    public class SetOpinionHiddenCommand : IRequest<ApiResponse<Unit>>
    {
        public required HideOpinionDto HideOpinionDto { get; set; }
    }
}
