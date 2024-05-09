namespace CollectionManagement.Domain.Entities.Likes;

public class LikeItem : Auditable
{
  public int ItemId { get; set; }

  // public virtual Item Item { get; set; } = default!;
  public Item? Item { get; set; }

  public int UserId { get; set; }

  // public virtual User User { get; set; } = default!;
  public User? User { get; set; }
}
