using Domain.Enums.Posts;

namespace Application.DTOs.Posts;

public class PromotionDto
{
    public required string Id { get; set; }

    public PostPromotionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}