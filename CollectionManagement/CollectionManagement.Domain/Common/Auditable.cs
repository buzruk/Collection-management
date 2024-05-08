namespace CollectionManagement.Domain.Common;

internal abstract class Auditable
{
  [Required]
  public DateTime CreatedAt { get; set; }

  [Required]
  public DateTime UpdatedAt { get; set; }
}
