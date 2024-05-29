namespace CollectionManagement.Domain.Entities;

public class Tag : Auditable
{
    public string Name
    {
        get;
        set;
    } = string.Empty;

    public int ItemId
    {
        get;
        set;
    }

    public List<Item> Items
    {
        get;
        set;
    } = [];
}
