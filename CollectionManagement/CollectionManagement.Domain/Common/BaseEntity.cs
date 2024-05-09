namespace CollectionManagement.Domain.Common;

public abstract class BaseEntity
{
  [Key, Required]
  public int Id { get; set; }
}
