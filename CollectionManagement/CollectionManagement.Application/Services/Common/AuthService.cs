using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services.Common;

public class AuthService(UserManager<User> userManager,
                         SignInManager<User> signInManager,
                         IConfiguration config,
                         IHttpContextAccessor httpContextAccessor,
                         IOneTimePasswordService oneTimePasswordService)
  : IAuthService
{
  private readonly IConfiguration _config = config.GetSection("Jwt");
  private readonly UserManager<User> _userManager = userManager;
  private readonly SignInManager<User> _signInManager = signInManager;
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  private readonly IOneTimePasswordService _oneTimePasswordService = oneTimePasswordService;

  /// <summary>
  /// Creates a new user account using user manager and adds the user to "User" role
  /// </summary>
  /// <param name="dto"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="FurnitureException"></exception>
  public async Task RegisterAsync(UserRegisterDto dto)
  {
    if (dto is null)
    {
      throw new ArgumentNullException(nameof(dto));
    }

    //if (!dto.IsValid())
    //{
    //  throw new CollectionException("Invalid data");
    //}

    //if (emailcheck is not null)
    //  throw new StatusCodeException(HttpStatusCode.Conflict, "Email alredy exist");

    var user = (User)dto;
    user.CreatedAt = TimeHelper.GetCurrentServerTime();
    user.UpdatedAt = TimeHelper.GetCurrentServerTime();
    user.Status = StatusType.Active;

    await _userManager.SetUserNameAsync(user, dto.UserName);
    var result = await _userManager.CreateAsync(user, dto.Password);
    if (result.Succeeded)
    {
      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      var confirmationLink = new UrlActionContext()
      {
        Action = "ConfirmEmail",
        Controller = "Auth",
        Values = new { token, email = user.Email },
        Protocol = _httpContextAccessor.HttpContext.Request.Scheme
      };
      var sendOtpDto = new SendOtpDto()
      {
        Email = user.Email!,
        Subject = "Confirm your account",
        Message = $"Confirm your registration by following the link: <a href='{confirmationLink}'>link</a>"
      };
      _oneTimePasswordService.SendEmail(sendOtpDto);
      result = await _userManager.AddToRoleAsync(user, dto.Role);
      if (!result.Succeeded)
      {
        throw new CollectionException($"Failed to add role: {string.Join("\n", result.Errors)}");
      }
    }
    else
    {
      throw new CollectionException($"Failed to create {dto.Role}: {string.Join("\n", result.Errors
                                                                            .Select(er => er.Description))}");
      //foreach (var error in result.Errors)
      //{
      //  ModelState.AddModelError(string.Empty, error.Description);
      //}
    }
  }


  /// <summary>
  /// Login a user using user manager and generate a JWT token
  /// </summary>
  /// <param name="dto"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="FurnitureException"></exception>
  public async Task<UserDto?> LoginAsync(UserLoginDto dto)
  {
    if (dto is null)
    {
      throw new ArgumentNullException(nameof(dto));
    }

    //if (!dto.IsValid())
    //{
    //  throw new CollectionException("Invalid data");
    //}

    var user = await _userManager.FindByNameAsync(dto.UserName);
    if (user is null)
    {
      throw new ArgumentNullException("User not found");
    }

    if (user.Status != StatusType.Blocked)
    {
      var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
      if (!isEmailConfirmed)
      {
        throw new CollectionException("Email not confirmed");
      }

      var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
      if (!result.Succeeded)
      {
        throw new CollectionException("Invalid password");
      }

      var roles = await _userManager.GetRolesAsync(user);

      var token = GenerateJwtToken(user.UserName!, user.Id, roles.ToList());
      var provider = _config["Jwt:Issuer"] ?? "";
      //await _userManager.RemoveAuthenticationTokenAsync(user, provider, "Token");
      await _userManager.SetAuthenticationTokenAsync(user, provider, "Token", token);

      return new UserDto()
      {
        UserName = user.UserName!,
        Token = token,
        ImagePath = user.Image,
        BirthDate = user.BirthDate,
        UserId = user.Id,
        Roles = roles.ToList()
      };
    }

    return null;
  }


  /// <summary>
  /// Logout a user by removing the JWT token from user manager
  /// </summary>
  /// <param name="dto"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  public async Task LogoutAsync(UserLoginDto dto)
  {
    var user = await _userManager.FindByNameAsync(dto.UserName);
    if (user is null)
    {
      throw new ArgumentNullException("User not found");
    }

    await _userManager.RemoveAuthenticationTokenAsync(user,
                                                      _config["Jwt:Issuer"] ?? "",
                                                      "Token");
  }

  /// <summary>
  /// Generates a JWT token using JWT security token handler
  /// </summary>
  /// <param name="fullName"></param>
  /// <param name="username"></param>
  /// <param name="roles"></param>
  /// <returns></returns>
  public string GenerateJwtToken(string username,
                                 string userId,
                                 List<string> roles)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "key"); // Same key as used in authentication configuration

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
        {
                new Claim(ClaimTypes.Name, username??""),
                //new Claim(ClaimTypes.GivenName, fullName),
                new Claim("userId", userId),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }),
      Expires = DateTime.UtcNow.AddMonths(1),
      Audience = _config["Jwt:Issuer"] ?? "",
      Issuer = _config["Jwt:Issuer"] ?? "",
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    foreach (var role in roles)
    {
      tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
    }

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}

