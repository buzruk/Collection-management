namespace CollectionManagement.Domain.Entities;

public sealed class User : IdentityUser
{
    public string Image
    {
        get;
        set;
    } = string.Empty;

    public DateTime BirthDate
    {
        get;
        set;
    }

    public string Role
    {
        get;
        set;
    } = string.Empty;

    public StatusType Status
    {
        get;
        set;
    } = StatusType.Active;

    [Required]
    public DateTime CreatedAt
    {
        get;
        set;
    }

    [Required]
    public DateTime UpdatedAt
    {
        get;
        set;
    }
}
