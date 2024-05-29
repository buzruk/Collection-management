using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Shared.ViewModels.CommentViewModels;

public class CommentViewModel
{
  public int Id { get; set; }

  public string Content { get; set; } = String.Empty;

  public int ItemId { get; set; }

  public string UserId { get; set; } = string.Empty;

  public static explicit operator Comment(CommentViewModel model)
  {
    return new Comment()
    {
      Id = model.Id,
      Content = model.Content,
      ItemId = model.ItemId,
      UserId = model.UserId,
    };
  }

  public static explicit operator CommentViewModel(Comment model)
  {
    return new CommentViewModel()
    {
      Id = model.Id,
      Content = model.Content,
      ItemId = model.ItemId,
      UserId = model.UserId,
    };
  }
}

