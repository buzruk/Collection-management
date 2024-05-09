namespace CollectionManagement.Shared.DTOs.Users;

public class UserResetPasswordDto
{
  [Required(ErrorMessage = "Email is required!"), Email]
  public string Email { get; set; } = string.Empty;

  [Required]
  public int Code { get; set; }

  [Required, StrongPassword]
  public string Password { get; set; } = string.Empty;
}

