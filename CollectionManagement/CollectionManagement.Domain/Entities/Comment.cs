namespace CollectionManagement.Domain.Entities;

public sealed class Comment : Auditable
{
  public string Content
  {
    get;
    set;
  } = string.Empty;

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
