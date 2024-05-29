namespace CollectionManagement.Shared.DTOs.Users;

public class UserLoginDto
{
  [Required(ErrorMessage = "Enter an Email!")]
  [Email]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Enter an UserName!")]
  public string UserName { get; set; } = string.Empty;

  public string Role { get; set; } = string.Empty;

  public bool RememberMe { get; set; }

  [Required(ErrorMessage = "Enter a password!")]
  [StrongPassword]
  public string Password { get; set; } = string.Empty;
}
