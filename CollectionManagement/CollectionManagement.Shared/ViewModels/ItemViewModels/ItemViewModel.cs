using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Shared.ViewModels.ItemViewModels;

public class ItemViewModel
{
  public int Id { get; set; }

  public string Name { get; set; } = String.Empty;

  public string ImagePath { get; set; } = String.Empty;

  public string Description { get; set; } = String.Empty;

  public int LikeCount { get; set; }

  public bool IsLiked { get; set; }

  public int CollectionId { get; set; }

  public int CommentCount { get; set; }

  public string UserId { get; set; } = string.Empty;

  public Dictionary<string, object>? CustomFieldValues { get; set; }

  public static explicit operator Item(ItemViewModel model)
  {
    return new Item()
    {
      Id = model.Id,
      Name = model.Name,
      Image = model.ImagePath,
      Description = model.Description,
      CollectionId = model.CollectionId,
      UserId = model.UserId,
    };
  }

  public static explicit operator ItemViewModel(Item model)
  {
    return new ItemViewModel()
    {
      Id = model.Id,
      Name = model.Name,
      ImagePath = model.Image,
      Description = model.Description,
      CollectionId = model.CollectionId,
      UserId = model.UserId,
    };
  }
}

