namespace CollectionManagement.Shared.DTOs.Users;

public class SendToEmailDto
{
    [Required(ErrorMessage = "Email is required!"), Email]

    public string Email { get; set; } = string.Empty;
}

