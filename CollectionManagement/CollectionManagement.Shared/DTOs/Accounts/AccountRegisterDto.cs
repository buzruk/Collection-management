namespace CollectionManagement.Shared.DTOs.Accounts;

public class AccountRegisterDto : AccountLoginDto
{
  [Required(ErrorMessage = "Enter a name!")]

  public string UserName { get; set; } = String.Empty;

  public DateTime BirthDate { get; set; }
}

