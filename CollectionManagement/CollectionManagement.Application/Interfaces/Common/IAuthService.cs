namespace CollectionManagement.Application.Interfaces.Common;

public interface IAuthService
{
  string GenerateJwtToken(string username,
                          string userId,
                          List<string> roles);

  Task RegisterAsync(UserRegisterDto dto);

  Task<UserDto?> LoginAsync(UserLoginDto dto);

  Task LogoutAsync(UserLoginDto dto);
}

