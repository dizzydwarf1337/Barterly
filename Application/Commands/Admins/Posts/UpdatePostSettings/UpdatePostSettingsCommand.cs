using Application.Core.MediatR.Requests;
using Domain.Enums.Posts;
using MediatR;

namespace Application.Commands.Admins.Posts.UpdatePostSettings;

public class UpdatePostSettingsCommand : AdminRequest<Unit>
{
    public Guid Id { get; set; }
    public bool IsHidden { get; set; }
    public bool IsDeleted { get; set; }
    public PostStatusType PostStatusType { get; set; }
    public string? RejectionMessage { get; set; }
}