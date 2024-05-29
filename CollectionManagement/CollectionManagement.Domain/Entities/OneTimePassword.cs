namespace CollectionManagement.Domain.Entities;

public class OneTimePassword : BaseEntity
{
    public string Email
    {
        get;
        set;
    } = string.Empty;

    public string Code
    {
        get;
        set;
    } = string.Empty;

    public DateTime ExpirationDate
    {
        get;
        set;
    } = DateTime.UtcNow.AddMinutes(10); // 10 minutes for development mode
}
