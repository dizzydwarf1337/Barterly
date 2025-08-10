namespace Application.DTOs.Users;

public class UserSettingsDto
{
    public bool IsHidden { get; set; }

    public bool IsBanned { get; set; }

    public bool IsPostRestricted { get; set; }

    public bool IsOpinionRestricted { get; set; }

    public bool IsChatRestricted { get; set; }
}