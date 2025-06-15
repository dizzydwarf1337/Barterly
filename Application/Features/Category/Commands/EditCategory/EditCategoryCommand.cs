using API.Core.ApiResponse;
using Application.DTOs.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Commands.EditCategory
{
    public class EditCategoryCommand : IRequest<ApiResponse<Unit>>
    {
        public required CategoryDto category { get; set; }
    }
}
