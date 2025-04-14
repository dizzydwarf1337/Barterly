using API.Core.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.ResendEmailConfirm
{
    public class ResendEmailConfirmCommand : IRequest<ApiResponse<Unit>>
    {
        public string Email { get; set; }
    }
}
