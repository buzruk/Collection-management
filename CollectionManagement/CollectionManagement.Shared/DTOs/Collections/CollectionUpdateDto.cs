namespace CollectionManagement.Shared.DTOs.Collections;

public class CollectionUpdateDto
{
  public int Id { get; set; }
  [Required]
  public string Name { get; set; } = string.Empty;
  [Required]
  public string Description { get; set; } = string.Empty;
  [Required]
  public TopicType Topics { get; set; } = TopicType.Other;
  public string CustomFields { get; set; } = string.Empty;
  public IFormFile? Image { get; set; }
  public string ImagePath { get; set; } = string.Empty;
  public int CostomFieldId { get; set; }

  //public static implicit operator Collection(CollectionUpdateDto v)
  //{
  //  return new Collection
  //  {
  //    Id = v.Id,
  //    Name = v.Name,
  //    Description = v.Description,
  //    Topics = v.Topics,
  //    Image = v.ImagePath,
  //  };
  //}
}

