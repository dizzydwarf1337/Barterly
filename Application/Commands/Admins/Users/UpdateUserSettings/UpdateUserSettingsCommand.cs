using Application.Core.MediatR.Requests;
using MediatR;

namespace Application.Commands.Admins.Users.UpdateUserSettings;

public class UpdateUserSettingsCommand : AdminRequest<Unit>
{
    public Guid Id { get; set; }
    public bool IsHidden { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsBanned { get; set; }
    public bool IsChatRestricted { get; set; }
    public bool IsOpinionRestricted { get; set; }
    public bool IsPostRestricted { get; set; }
}