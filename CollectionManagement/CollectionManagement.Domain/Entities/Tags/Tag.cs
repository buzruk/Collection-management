namespace CollectionManagement.Domain.Entities.Tags;

internal class Tag
{
  public string Name { get; set; } = string.Empty;

  public int ItemId { get; set; }

  // public virtual List<Item> Items { get; set; } = default!;
  public List<Item> Items { get; set; } = [];
}
