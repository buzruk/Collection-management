namespace CollectionManagement.Domain.Entities;

public class Item : Auditable
{
  public string Name
  {
    get;
    set;
  } = string.Empty;

  public string Description
  {
    get;
    set;
  } = string.Empty;

  public string Image
  {
    get;
    set;
  } = string.Empty;

  public int TagId
  {
    get;
    set;
  }

  public List<Tag> Tags
  {
    get;
    set;
  } = [];

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
