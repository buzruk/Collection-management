using Microsoft.AspNetCore.Identity;

namespace CollectionManagement.Domain.Entities.Users;

public sealed class User : IdentityUser
{
  [Key, Required]
  public int Id { get; set; }
  public string UserName { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public string Image { get; set; } = string.Empty;

  public DateTime BirthDate { get; set; }

  public string PasswordHash { get; set; } = string.Empty;

  public string Salt { get; set; } = string.Empty;
  public string UserRole { get; set; } = string.Empty;

  public StatusType Status { get; set; } = StatusType.Active;

  [Required]
  public DateTime CreatedAt { get; set; }

  [Required]
  public DateTime UpdatedAt { get; set; }
}
