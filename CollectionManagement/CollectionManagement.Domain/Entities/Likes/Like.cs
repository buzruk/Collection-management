namespace CollectionManagement.Domain.Entities.Likes;

public class Like : Auditable
{
  public int CollectionId { get; set; }

  // public virtual Collection Collection { get; set; } = default!;
  public Collection? Collection { get; set; }

  public int UserId { get; set; }

  // public virtual User User { get; set; } = default!;
  public User? User { get; set; }
}
