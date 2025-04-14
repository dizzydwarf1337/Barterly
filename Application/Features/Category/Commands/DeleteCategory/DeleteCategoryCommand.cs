using API.Core.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<ApiResponse<Unit>>
    {
        public string CategoryId { get; set; }
    }
}
