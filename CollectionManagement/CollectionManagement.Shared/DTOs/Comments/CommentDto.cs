namespace CollectionManagement.Shared.DTOs.Comments;

public class CommentDto
{
    public int Id { get; set; }

    [Comment]
    [Required]
    public string CommentText { get; set; } = string.Empty;

    public int ItemId { get; set; }
}

