namespace CollectionManagement.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminsController(IUserService userService,
                              IAuthService authService)
    : Controller
{
  private readonly IUserService _userService = userService;
  private readonly IAuthService _authService = authService;

  /// <summary>
  /// [SuperAdmin] Yangi admin yaratish
  /// </summary>
  /// <param name="request"></param>
  /// <returns></returns>
  /// HAVE TO DOCUMENT ALL ACIONS LIKE THAT
  /// <response code="200">Created new admin</response>
  /// <response code="400">Created new admin</response>
  /// <response code="401">Created new admin</response>
  /// <response code="404">Created new admin</response>
  /// <response code="500">Created new admin</response>
  [HttpPost("create")]
  //[Authorize(Roles = RoleConstants.SuperAdmin)]
  [Authorize(Roles = RoleConstants.SuperAdmin)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateAdmin([FromBody] UserRegisterDto request)
  {
    try
    {
      //await _userService.CreateAdminAsync(request);
      await _authService.RegisterAsync(request);
      //return Ok();
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

  [HttpPut("update")]
  //[Authorize(Roles = $"{RoleConstants.SuperAdmin}, {RoleConstants.Admin}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> UpdateAdmin([FromBody] UserUpdateDto request)
  {
    try
    {
      //await _userService.UpdateUserAsync(request);
      await _userService.UpdateAsync(request);
      //return Ok();
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

  [HttpGet("get-all")]
  //[Authorize(Roles = RoleConstants.SuperAdmin)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllAdmins()
  {
    try
    {
      //var admins = await _userService.GetUsersAsync(UserRoles.Admin);
      var @params = new PaginationParams(1, 10);
      var admins = await _userService.GetPagedAsync(RoleConstants.Admin, @params);
      //return Ok(admins);
      return View(admins);
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

  [HttpDelete("{id}")]
  //[Authorize(Roles = RoleConstants.SuperAdmin)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteAdmin(string userId)
  {
    try
    {
      //await _userService.DeleteUserAsync(id);
      await _userService.RemoveAsync(userId);
      //return Ok();
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

  [HttpPost("block")]
  public async Task<IActionResult> Block(string id)
  {
    await _userService.BlockAsync(id);
    return RedirectToAction("Index", "Home");
  }

  [HttpPost("active")]
  public async Task<IActionResult> Active(string id)
  {
    await _userService.ActiveAsync(id);
    return RedirectToAction("Index", "Home");
  }

  [HttpDelete("deleteimage")]
  public async Task<IActionResult> DeleteImage(string id)
  {
    await _userService.DeleteProfileImageAsync(id);
    return View();
  }

  [HttpPut("activate/{userId}")]
  //[Authorize(Roles = RoleConstants.SuperAdmin)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> ActivateAdmin(string userId)
  {
    try
    {
      //await _userService.ActivateAdminAsync(userId);
      await _userService.ActiveAsync(userId);
      //return Ok();
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

  [HttpPut("reset-password/{userId}")]
  //[Authorize(Roles = RoleConstants.SuperAdmin)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  //public async Task<IActionResult> ResetPassword(string userId)
  public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
  {
    try
    {
      //ResetPasswordDto
      //
      //UserId
      //Token
      //NewPassword
      await _userService.ResetPassword(dto);
      //return Ok();
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
}

