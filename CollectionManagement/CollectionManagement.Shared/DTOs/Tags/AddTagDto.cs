namespace CollectionManagement.Shared.DTOs.Tags;

public class AddTagDto
{
  public IEnumerable<string> Tags { get; set; }
  public Item Item { get; set; }
}

