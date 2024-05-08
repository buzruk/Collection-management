namespace CollectionManagement.Domain.Entities;

internal class Person : Auditable
{
  public string UserName { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public string Image { get; set; } = string.Empty;

  public DateTime BirthDate { get; set; }

  public string PasswordHash { get; set; } = string.Empty;

  public string Salt { get; set; } = string.Empty;
}
