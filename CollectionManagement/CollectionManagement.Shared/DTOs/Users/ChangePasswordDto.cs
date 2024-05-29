namespace CollectionManagement.Shared.DTOs.Users;

public class ChangePasswordDto
{
  [Required(ErrorMessage = "Enter username!")]
  public string UserName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Enter a password!")]
  [StrongPassword]
  public string OldPassword { get; set; } = string.Empty;

  [Required(ErrorMessage = "Enter a password!")]
  [StrongPassword]
  public string NewPassword { get; set; } = string.Empty;
}
