namespace CollectionManagement.Domain.Entities;

public class Like : Auditable
{
  public int CollectionId
  {
    get;
    set;
  }

  public Collection? Collection
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
