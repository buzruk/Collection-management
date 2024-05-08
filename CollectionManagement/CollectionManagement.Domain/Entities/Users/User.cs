namespace CollectionManagement.Domain.Entities.Users;

internal sealed class User : Person
{
  public string UserRole { get; set; } = string.Empty;

  public StatusType Status { get; set; } = StatusType.Active;
}
