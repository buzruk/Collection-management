namespace CollectionManagement.Domain.Entities.Admins;

public sealed class Admin : Person
{
  public string Address { get; set; } = string.Empty;

  public string AdminRole { get; set; } = string.Empty;

  public StatusType Status { get; set; } = StatusType.Active;
}
