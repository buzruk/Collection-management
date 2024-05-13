namespace CollectionManagement.Infrastructure.Services.Common;

public class AuthService(IConfiguration config) 
  : IAuthService
{
  private readonly IConfiguration _config = config.GetSection("Jwt");

  public string GenerateToken(User user, string role)
  {
    var claims = new[]
       {
              new Claim("Id", user.Id.ToString()),
              new Claim("UserName", user.UserName),
              new Claim("ImagePath", (user.Image is null) ? "" : user.Image),
              new Claim(ClaimTypes.Role, $"{role}")
          };

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]!));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

    var tokenDescriptor = new JwtSecurityToken(_config["Issuer"], _config["Audience"], claims,
        expires: DateTime.Now.AddMinutes(double.Parse(_config["Lifetime"]!)),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
  }
}

