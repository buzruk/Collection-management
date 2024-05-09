namespace CollectionManagement.Shared.DTOs.Items;

public class ItemUpdateDto
{
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  [Required]
  public string Description { get; set; } = string.Empty;

  public IFormFile? Image { get; set; }

  public string ImagePath { get; set; } = string.Empty;

  public Dictionary<string, object>? CustomFieldValues { get; set; }

  public int CollectionId { get; set; }
}

