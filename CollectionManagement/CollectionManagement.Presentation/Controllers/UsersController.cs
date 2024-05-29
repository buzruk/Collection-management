namespace CollectionManagement.Presentation.Controllers;

//[Route("users")]
////[Authorize]
//public class UsersController(IUserService userService)
//  : Controller
//{
//    private readonly IUserService _userService = userService;
//    private readonly int _pageSize = 10;

//    [HttpGet]
//    public IActionResult Index()
//    {
//        return View();
//    }

//    [HttpDelete("delete")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        try
//        {
//            var res = await _userService.DeleteAsync(id);
//            if (res)
//            {
//                return RedirectToAction("Index", res);
//            }
//            else
//            {
//                TempData["Error"] = "Failed to delete user";
//                return RedirectToAction("Index", "User");
//            }
//        }
//        catch (Exception ex)
//        {
//            TempData["Error"] = ex.Message;
//            return RedirectToAction("Index", "User");
//        }
//    }

//    [HttpDelete("deleteimage")]
//    public async Task<IActionResult> DeleteImage(int id)
//    {
//        try
//        {
//            var res = await _userService.DeleteImageAsync(id);
//            if (res)
//            {
//                return RedirectToAction("Index", res);
//            }
//            else
//            {
//                TempData["Error"] = "Failed to delete image";
//                return RedirectToAction("Index", "User");
//            }
//        }
//        catch (Exception ex)
//        {
//            TempData["Error"] = ex.Message;
//            return RedirectToAction("Index", "User");
//        }
//    }

//    [HttpPut("updateimage")]
//    public async Task<IActionResult> UpdateImage(int id, [FromForm] IFormFile image)
//    {
//        try
//        {
//            var res = await _userService.UpdateImageAsync(id, image);
//            if (res)
//            {
//                return RedirectToAction("Index", res);
//            }
//            else
//            {
//                TempData["Error"] = "Failed to update image";
//                return RedirectToAction("Index", "Home");
//            }
//        }
//        catch (Exception ex)
//        {
//            TempData["Error"] = ex.Message;
//            return RedirectToAction("Index", "Home");
//        }
//    }

//    [HttpPut("updateuser")]
//    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
//    {
//        try
//        {
//            var res = await _userService.UpdateAsync(id, userDto);
//            if (res)
//            {
//                return RedirectToAction("Index", res);
//            }
//            else
//            {
//                TempData["Error"] = "Failed to update user";
//                return RedirectToAction("Index", "Home");
//            }
//        }
//        catch (Exception ex)
//        {
//            TempData["Error"] = ex.Message;
//            return RedirectToAction("Index", "Home");
//        }
//    }

//    [HttpPut("updatepassword")]
//    public async Task<IActionResult> UpdatePassword(int id, ChangePasswordDto password)
//    {
//        try
//        {
//            var res = await _userService.UpdatePasswordAsync(id, password);
//            if (res)
//            {
//                return RedirectToAction("Index", res);
//            }
//            else
//            {
//                TempData["Error"] = "Failed to update password";
//                return RedirectToAction("Index", "Home");
//            }
//        }
//        catch (Exception ex)
//        {
//            TempData["Error"] = ex.Message;
//            return RedirectToAction("Index", "Home");
//        }
//    }
//}

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = $"{RoleConstants.SuperAdmin}, {RoleConstants.Admin}")]
public class UsersController(IUserService userService)
    : Controller
{
  private readonly IUserService userService = userService;

  [HttpGet("all")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetUsers()
  {
    //var users = await userService.GetUsersAsync(UserRoles.User);
    var @params = new PaginationParams(1, 10);
    var users = await userService.GetPagedAsync(RoleConstants.User, @params);
    return Ok(users);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetUser(string id)
  {
    try
    {
      //var user = await userService.GetUserAsync(id);
      var user = await userService.GetAsync(id);
      return Ok(user);
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

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteAdmin(string id)
  {
    try
    {
      //await userService.DeleteUserAsync(id);
      await userService.RemoveAsync(id);
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
}
