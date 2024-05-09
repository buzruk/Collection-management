namespace CollectionManagement.Shared.DTOs.Collections;

public class CollectionDto
{
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  [Required]
  public TopicType Topics { get; set; } = TopicType.Other;

  public IFormFile? Image { get; set; }

  public int CustomFieldId { get; set; }

  public Dictionary<string, object>? CustomFieldValues { get; set; }
}

