namespace CollectionManagement.Domain.Entities.CustomFields;

public sealed class CustomField : Auditable
{
  public string Name { get; set; } = string.Empty;

  public FieldType Type { get; set; } = FieldType.String;

  public int CollectionId { get; set; }

  // public virtual Collection Collection { get; set; } = default!;
  public Collection? Collection { get; set; }

  // public int ItemId { get; set; }
  // public virtual Item Item { get; set; } = default!;
}
