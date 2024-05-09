namespace CollectionManagement.Shared.ViewModels.CommentViewModels;

public class CommentViewModel
{
  public int Id { get; set; }

  public string Content { get; set; } = String.Empty;

  public int ItemId { get; set; }

  public int UserId { get; set; }
}

