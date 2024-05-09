namespace CollectionManagement.Domain.Common;

public abstract class Auditable
{
  [Required]
  public DateTime CreatedAt { get; set; }

  [Required]
  public DateTime UpdatedAt { get; set; }
}
