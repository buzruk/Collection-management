namespace CollectionManagement.Shared.DTOs.OneTimePassword;

public class SendOtpDto
{
  public string Email { get; set; } = string.Empty;
  public string Subject { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
}
