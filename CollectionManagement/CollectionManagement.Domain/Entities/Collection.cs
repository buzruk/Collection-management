namespace CollectionManagement.Domain.Entities;

public sealed class Collection : Auditable
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

    public TopicType Topic
    {
        get;
        set;
    } = TopicType.Other;

    public string Image
    {
        get;
        set;
    } = string.Empty;

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

    public int CustomFieldId
    {
        get;
        set;
    }

    public List<CustomField> CustomFields
    {
        get;
        set;
    } = [];
}
