namespace CollectionManagement.Presentation.Controllers;

[Route("collections")]
public class CollectionsController(ICollectionService collectionService,
                                   IUserService userService,
                                   IHttpContextAccessor httpContextAccessor,
                                   ILikeService likeService)
  : Controller
{
  private readonly ILikeService _likeService = likeService;
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  private readonly IUserService _userService = userService;
  private readonly ICollectionService _collectionService = collectionService;
  private readonly int _pageSize = 10;

  [HttpGet]
  public async Task<IActionResult> Index()
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    var result = await _collectionService.GetPagedAsync(new PaginationParams(1, _pageSize));
    return View(result);
  }

  [HttpGet("search")]
  public async Task<IActionResult> SearchAsync(string name, int page = 1)
  {
    try
    {
      ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
      var result = await _collectionService.SearchAsync(new PaginationParams(page, _pageSize), name);
      return View("Index", result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  //[Authorize]
  [HttpGet("create")]
  public IActionResult Create()
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    return View("Create");
  }

  //[Authorize]
  [HttpPost("create")]
  public async Task<IActionResult> CreateAsync(CollectionDto collectionCreateDto)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    try
    {
      var success = await _collectionService.AddAsync(collectionCreateDto);
      SetTempMessage(success, "Collection created successfully", "Failed");
      if (success is true)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while creating the collection: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Home"); // Redirect to the home page with an error message
    }
  }

  //[Authorize]
  [HttpPost("delete")]
  public async Task<IActionResult> DeleteAsync(int id)
  {
    try
    {
      var success = await _collectionService.RemoveAsync(id);
      SetTempMessage(success, "Collection deleted successfully", "Failed");
      return RedirectToAction("Index", "Collections");
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while deleting the collection: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Home"); // Redirect to the home page with an error message
    }
  }
  //[Authorize]
  [HttpGet("update")]
  public ActionResult Update(int id)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    ViewBag.Id = id;
    return View("Edit");
  }

  //[Authorize]
  [HttpPost("update")]
  public async Task<IActionResult> UpdateAsync(int id, CollectionUpdateDto collectionUpdateDto)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    try
    {
      var success = await _collectionService.UpdateAsync(id, collectionUpdateDto);
      SetTempMessage(success, "Collection updated successfully", "Failed");
      if (success is true)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while updating the collection: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Collections"); // Redirect to the home page with an error message
    }
  }


  [HttpGet("gettop")]
  public async Task<IActionResult> TopCollection(int page = 1)
  {
    try
    {
      var res = await _collectionService.TopCollection(new PaginationParams(page, _pageSize));
      return View(res);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  //[Authorize]
  [HttpGet("likecollection")]
  public async Task<IActionResult> LikeCollection(int collectionId)
  {
    try
    {
      var res = await _likeService.ToggleCollection(collectionId);
      if (res)
      {
        return RedirectToAction("Index", "Home");
      }
      else
      {
        TempData["Error"] = "Failed to like collection";
        return RedirectToAction("Index", "Home");
      }
    }
    catch (Exception ex)
    {
      TempData["Error"] = ex.Message;
      return RedirectToAction("Index", "Home");
    }
  }
  private void SetTempMessage(bool success, string successMessage, string errorMessage)
  {
    TempData[success ? "SuccessMessage" : "ErrorMessage"] = success ? successMessage : errorMessage;
  }
}

