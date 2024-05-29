namespace CollectionManagement.Domain.Entities;

public class LikeItem : Auditable
{
  public int ItemId
  {
    get;
    set;
  }

  public Item? Item
  {
    get;
    set;
  }

  public string UserId
  {
    get;
    set;
  } = string.Empty;

  public User? User
  {
    get;
    set;
  }
}
