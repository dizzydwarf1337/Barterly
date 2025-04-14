using API.Core.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ApiResponse<Unit>>
    {
        public string userMail { get; set; }
        public string token { get; set; }
    }
}
