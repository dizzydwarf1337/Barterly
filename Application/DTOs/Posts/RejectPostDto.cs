namespace Application.DTOs.Posts;

public class RejectPostDto
{
    public required string postId { get; set; }
    public required string reason { get; set; }
}