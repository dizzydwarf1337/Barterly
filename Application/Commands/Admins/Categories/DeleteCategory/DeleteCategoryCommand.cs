using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Categories.DeleteCategory;

public class DeleteCategoryCommand : AdminRequest<Unit>
{
    public required Guid CategoryId { get; set; }
}