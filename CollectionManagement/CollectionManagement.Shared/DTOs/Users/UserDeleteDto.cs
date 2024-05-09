namespace CollectionManagement.Shared.DTOs.Users;

public class UserDeleteDto
{
  [Required(ErrorMessage = "Enter your password")]
  public string Password { get; set; } = default!;
}

