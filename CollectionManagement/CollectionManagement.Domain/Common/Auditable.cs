namespace CollectionManagement.Domain.Common;

public abstract class Auditable : BaseEntity
{
  [Required]
  public DateTime CreatedAt { get; set; }

  [Required]
  public DateTime UpdatedAt { get; set; }
}
