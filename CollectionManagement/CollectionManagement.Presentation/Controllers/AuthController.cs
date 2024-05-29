namespace CollectionManagement.Presentation.Controllers;

//[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
public class AuthController(IUserService userService,
                            IAuthService authService,
                            IOneTimePasswordService oneTimePasswordService,
                            UserManager<User> userManager)
    : Controller
{
  private readonly IUserService _userService = userService;
  private readonly IAuthService _authService = authService;
  private readonly IOneTimePasswordService _oneTimePasswordService = oneTimePasswordService;
  private readonly UserManager<User> _userManager = userManager;

  /// <summary>
  /// [All] Login qilish (Telefon raqam tasdiqlangan bo'lishi kerak)
  /// </summary>
  [HttpPost]
  [AllowAnonymous]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login( UserLoginDto request)
  {
    if (ModelState.IsValid)
    {
      try
      {
        var response = await _authService.LoginAsync(request);

        if (response is not null && response.Token is not null)
        {
          HttpContext.Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions()
          {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
          });
        }

        return RedirectToAction("Index", "Home", response);
      }
      catch (ArgumentNullException ex)
      {
        return NotFound(ex.Message);
      }
      catch (CollectionException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }
    else
    {
      return View(request);
    }
  }

  [HttpGet]
  [AllowAnonymous]
  public IActionResult Login() => View("Login");

  /// <summary>
  /// [User] Ro'yxatdan o'tish
  /// </summary>
  [HttpPost]
  [AllowAnonymous]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
  {
    try
    {
      await _authService.RegisterAsync(request);

      //return Content("To complete registration, check your email and follow the link provided in the letter");
      return View("Login");
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  [HttpGet]
  [AllowAnonymous]
  public IActionResult Register() => View("Register");


  /// <summary>
  /// [All] Telefon raqamni tasdiqlash uchun sms yuborish
  /// </summary>
  [HttpPost]
  [AllowAnonymous]
  [ValidateAntiForgeryToken]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult SendOtp([FromBody] SendOtpDto request)
  {
    try
    {
      _oneTimePasswordService.SendEmail(request);
      return View("SuccessRegistration");
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  /// <summary>
  /// [All] Kodni tasdiqlash 2 daqiqa
  /// </summary>
  [HttpPost]
  [AllowAnonymous]
  [ValidateAntiForgeryToken]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto request)
  {
    //<h1>ConfirmEmail</h1>

    //<div>
    //  <p>
    //    Thank you for confirming your email.
    //  </p>
    //</div>

    if (request.Email == null || request.Code == null)
      return View("Error");

    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user == null)
      return View("Error");

    var result = await _userManager.ConfirmEmailAsync(user, request.Code);

    //return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    if (result.Succeeded)
      return RedirectToAction("Index", "Home");
    else
      return View("Error");
  }


  [HttpGet]
  public IActionResult SuccessRegistration()
  {
    //<h1>SuccessRegistration</h1>

    //<p>
    //  Please check your email for the verification action.
    //</p>
    return View();
  }

  /// <summary>
  /// [All] Logout qilish joriy tokenni o'chiradi
  /// </summary>
  [HttpPut]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Logout([FromBody] UserLoginDto request)
  {
    try
    {
      await _authService.LogoutAsync(request);

      HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
      {
        Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
      });
      return RedirectToAction("login", new { area = "" });
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  /// <summary>
  /// [All] Telefon raqamni o'zgartirish
  /// </summary>
  [HttpPut]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
  {
    try
    {
      await _userService.ChangePasswordAsync(request);
      return View();
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  /// <summary>
  /// [SuperAdmin , Admin] Foydalanuvchini o'chirish
  /// </summary>
  /// <param name="dto"></param>
  /// <returns></returns>
  [HttpDelete]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  // Development uchun ochiq qoldirildi
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Delete(string id)
  {
    try
    {
      await _userService.RemoveAsync(id);
      return Ok();
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  [HttpPost]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ValidateAntiForgeryToken]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> SetProfileImage(SetAvatarDto request)
  {
    try
    {
      await _userService.SetProfileImageAsync(request);
      return Ok();
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  [HttpPut]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> ChangeProfileImage(SetAvatarDto request)
  {
    try
    {
      await _userService.UpdateProfileImageAsync(request);
      return Ok();
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  [HttpDelete]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteProfileImage([FromRoute] string id)
  {
    try
    {
      //await _userService.DeleteProfilePictureAsync(userId);
      await _userService.DeleteProfileImageAsync(id);
      return Ok();
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  [HttpGet]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public IActionResult ValidateToken()
  {
    return Ok();
  }

  [HttpPut]
  //[Authorize(Roles = $"{RoleConstants.User}, {RoleConstants.Admin}, {RoleConstants.SuperAdmin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto request)
  {
    try
    {
      //await _userService.UpdateUserAsync(dto);
      await _userService.UpdateAsync(request);
      return View("Index", "Home");
    }
    catch (ArgumentNullException ex)
    {
      return NotFound(ex.Message);
    }
    catch (CollectionException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }
}
