namespace Application.DTOs.General.Opinions;

public class HideOpinionDto
{
    public required string OpinionId { get; set; }
    public bool isHidden { get; set; }
}