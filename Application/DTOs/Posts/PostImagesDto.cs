namespace Application.DTOs.Posts;

public class PostImagesDto
{
    public required string postId { get; set; }
    public string? MainImageUrl { get; set; }
    public string[]? SecondaryImagesUrl { get; set; }
}