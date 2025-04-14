using API.Core.ApiResponse;
using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Commands.AddCategory
{

        public class AddCategoryCommand : IRequest<ApiResponse<Unit>>
        {
            public CategoryDto category;
        }
        
    }
