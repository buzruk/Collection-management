namespace CollectionManagement.Shared.DTOs.Admins;

public class AdminRegisterDto : AccountRegisterDto
{
  public static explicit operator Admin(AdminRegisterDto dto)
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

