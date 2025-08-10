using Domain.Enums.Posts;

namespace Domain.Entities.Posts;

public class PostSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PostId { get; set; }
    public Post Post { get; set; } = default!;
    public bool IsHidden { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public PostStatusType postStatusType { get; set; } = PostStatusType.UnderReview;
    public string? RejectionMessage { get; set; }
}