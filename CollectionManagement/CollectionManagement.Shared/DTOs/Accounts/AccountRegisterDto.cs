namespace CollectionManagement.Shared.DTOs.Accounts;

public class AccountRegisterDto : AccountLoginDto
{
  [Required(ErrorMessage = "Enter a name!")]

  public string UserName { get; set; } = String.Empty;

  public DateTime BirthDate { get; set; }

  public static explicit operator User(AccountRegisterDto dto)
  {
    return new()
    {
      UserName = dto.UserName,
      BirthDate = dto.BirthDate,
      Email = dto.Email,
      PasswordHash = PasswordHasher.Hash(dto.Password).Hash,
      Salt = PasswordHasher.Hash(dto.Password).Salt
    };
  }
}

