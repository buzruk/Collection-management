using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Shared.ViewModels.CollectionViewModels;

public class CollectionViewModel
{
  public int Id { get; set; }

  public string Name { get; set; } = String.Empty;

  public string Description { get; set; } = String.Empty;

  public TopicType Topics { get; set; } = TopicType.Other;

  public string ImagePath { get; set; } = String.Empty;

  public DateTime CreatedAt { get; set; } = default!;

  public DateTime LastUpdatedAt { get; set; } = default!;

  public int LikeCount { get; set; } = default!;

  public bool IsLiked { get; set; }

  public string UserId { get; set; } = string.Empty;

  public int CustomFieldId { get; set; }

  public static explicit operator CollectionViewModel(Collection model)
  {
    return new CollectionViewModel()
    {
      Id = model.Id,
      Name = model.Name,
      ImagePath = model.Image,
      Description = model.Description,
      Topics = model.Topic,
      CreatedAt = model.CreatedAt,
      LastUpdatedAt = model.UpdatedAt,
    };
  }
}

