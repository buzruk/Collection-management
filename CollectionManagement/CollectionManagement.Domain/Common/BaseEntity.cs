namespace CollectionManagement.Domain.Common;

internal abstract class BaseEntity
{
  [Key, Required]
  public int Id { get; set; }
}
