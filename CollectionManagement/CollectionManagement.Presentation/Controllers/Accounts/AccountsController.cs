namespace CollectionManagement.Presentation.Controllers.Accounts;

[Route("accounts")]
public class AccountsController(IAccountService accountService) 
  : Controller
{
  private readonly IAccountService _service = accountService;

  [HttpGet("adminregister")]
  public IActionResult AdminRegister() => View("AdminRegister");

  [HttpPost("adminregister")]
  public async Task<IActionResult> AdminRegisterAsync(AdminRegisterDto adminRegisterDto, CancellationToken cancellationToken = default)
  {
    if (ModelState.IsValid)
    {
      bool result = await _service.AdminRegisterAsync(adminRegisterDto);
      if (result)
      {
        return RedirectToAction("login", "accounts", new { area = "" });
      }
      else
      {
        return AdminRegister();
      }
    }
    else return AdminRegister();
  }

  [HttpGet("register")]
  public IActionResult Register() => View("Register");

  [HttpPost("register")]
  public async Task<IActionResult> UserRegisterAsync(AdminRegisterDto adminRegisterDto, CancellationToken cancellationToken = default)
  {
    if (ModelState.IsValid)
    {
      bool result = await _service.RegisterAsync(adminRegisterDto);
      if (result)
      {
        return RedirectToAction("login", "accounts", new { area = "" });
      }
      else
      {
        return Register();
      }
    }
    else return Register();
  }
  [HttpGet("login")]
  public IActionResult Login() => View("Login");

  [HttpPost("login")]
  public async Task<IActionResult> LoginAsync(AccountLoginDto accountLoginDto, CancellationToken cancellationToken = default)
  {
    if (ModelState.IsValid)
    {
      try
      {
        string token = await _service.LoginAsync(accountLoginDto);
        HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions()
        {
          HttpOnly = true,
          SameSite = SameSiteMode.Strict
        });
        return RedirectToAction("Index", "Home", new { area = "" });
      }
      catch (ModelErrorException modelError)
      {
        ModelState.AddModelError(modelError.Property, modelError.Message);
        return Login();
      }
      catch
      {
        return Login();
      }
    }
    else return Login();
  }

  [HttpGet("logout")]
  public IActionResult LogOut()
  {
    HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
    {
      Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
    });
    return RedirectToAction("login", "accounts", new { area = "" });
  }
}

