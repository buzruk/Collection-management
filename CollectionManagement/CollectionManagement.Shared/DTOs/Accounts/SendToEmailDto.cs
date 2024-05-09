namespace CollectionManagement.Shared.DTOs.Accounts;

public class SendToEmailDto
{
  [Required(ErrorMessage = "Email is required!"), EmailAttribute]

  public string Email { get; set; } = string.Empty;
}

