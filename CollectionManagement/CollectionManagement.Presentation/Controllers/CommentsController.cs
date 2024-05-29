namespace CollectionManagement.Presentation.Controllers;

[Route("comments")]
public class CommentsController(ICommentService commentService,
                                IHttpContextAccessor httpContextAccessor,
                                IUserService userService)
  : Controller
{
  private readonly IUserService _userService = userService;
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  private readonly ICommentService _commentService = commentService;
  private readonly int _pageSize = 10;

  [HttpGet]
  public async Task<IActionResult> Index(int id)
  {
    ViewBag.Id = id;
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    var res = await _commentService.GetPagedAsync(id, new PaginationParams(1, _pageSize));
    return View(res);
  }

  [HttpGet("create")]
  public IActionResult Create(int id)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    ViewBag.Id = id;
    return View("Create");
  }

  //[Authorize]
  [HttpPost("create")]
  public async Task<IActionResult> Create(int id, CommentDto commentDto)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    commentDto.ItemId = id;
    try
    {
      var success = await _commentService.AddAsync(commentDto);
      SetTempMessage(success, "Comment created successfully", "Failed");
      return RedirectToAction("Index", "Comments", new { id });
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while creating the comment: {ex.Message}";
      return RedirectToAction("Index", "Home");
    }
  }

  //[Authorize]
  [HttpGet("delete")]
  public async Task<IActionResult> Delete(int id, int commentId)
  {
    ViewBag.Id = id;
    try
    {
      var success = await _commentService.RemoveAsync(commentId);
      SetTempMessage(success, "Comment deleted successfully", "Failed");
      //return RedirectToAction("Index", "Comments", id);
      return RedirectToAction("Index", "Comments", new { id });
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while deleting the comment: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Home"); // Redirect to the home page with an error message
    }
  }
  private void SetTempMessage(bool success, string successMessage, string errorMessage)
  {
    TempData[success ? "SuccessMessage" : "ErrorMessage"] = success ? successMessage : errorMessage;
  }
}

