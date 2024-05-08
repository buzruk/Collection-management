namespace CollectionManagement.Domain.Entities.Items;

internal class Item : Auditable
{
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public string Image { get; set; } = string.Empty;

  public int TagId { get; set; }

  // public virtual List<Tag> Tags { get; set; } = default!;
  public List<Tag> Tags { get; set; } = [];

  public int CollectionId { get; set; }

  // public virtual Collection Collection { get; set; } = default!;
  public Collection? Collection { get; set; }

  public int UserId { get; set; }

  // public virtual User User { get; set; } = default!;
  public User? User { get; set; }
}
