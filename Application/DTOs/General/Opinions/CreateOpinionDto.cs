using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.General.Opinions;

public class CreateOpinionDto
{
    public required string AuthorId { get; set; }
    public required string SubjectId { get; set; }

    [Length(5, 500, ErrorMessage = "Opinion content lenght shoud be between 5 and 500")]
    public required string Content { get; set; }

    [Range(1, 5)] public int Rate { get; set; }
    public required string OpinionType { get; set; }
}