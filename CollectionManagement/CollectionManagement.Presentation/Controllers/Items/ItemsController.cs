namespace CollectionManagement.Presentation.Controllers.Items;

[Route("items")]
public class ItemsController(ItemService itemService,
                             IHttpContextAccessor httpContextAccessor,
                             IUserService userService,
                             ILikeService likeService,
                             IIdentityService identityService,
                             ICollectionService collectionService) 
  : Controller
{
  private readonly int _pageSize = 10;
  private readonly ItemService _itemService= itemService;
  private readonly ICollectionService _collectionService = collectionService;
  private readonly IIdentityService _identityService = identityService;
  private readonly ILikeService _likeService = likeService;
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  private readonly IUserService _userService = userService;

  [HttpGet]
  public async Task<IActionResult> Index(int id, int page = 1)
  {
    try
    {
      ViewBag.collectionId = id;
      ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
      var items = await _itemService.GetPagedAsync(id, new PaginationParams(page, _pageSize));

      if (items is null)
        return View("Index");

      return View(items);
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while fetching items: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Home"); // Redirect to the home page with an error message
    }
  }

  [Authorize]
  [HttpGet("create")]
  public async Task<IActionResult> Create(int collectionId)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    var userId = _identityService.Id ?? 0;
    var res = await _collectionService.GetCollectionById(userId, collectionId);

    if (res == true)
    {
      ViewBag.collectionId = collectionId;
      return View("Create");
    }
    else
    {
      return RedirectToAction("Index", "Home");
    }
  }

  [Authorize]
  [HttpPost("create")]
  public async Task<IActionResult> CreateAsync(ItemDto itemDto)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    var id = itemDto.CollectionId;
    try
    {
      var success = await _itemService.CreateItemAsync(itemDto);
      SetTempMessage(success, "Item created successfully", "Failed");
      return RedirectToAction("Index", new { id });
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while creating the item: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", new { id }); // Redirect to the home page with an error message
    }
  }

  [Authorize]
  [HttpPost("delete")]
  public async Task<IActionResult> DeleteAsync(int id, int itemId)
  {
    ViewBag.CollectionId = id;
    try
    {
      var success = await _itemService.DeleteItemAsync(itemId);
      SetTempMessage(success, "Item deleted successfully", "Failed");
      return RedirectToAction("Index", "Items", new { id });
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while deleting the item: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Items", new { id });
    }
  }

  [Authorize]
  [HttpGet("update")]
  public IActionResult Update(int id)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    ViewBag.Id = id;
    return View("Edit");
  }

  [Authorize]
  [HttpPost("update")]
  public async Task<IActionResult> UpdateAsync(int id, ItemUpdateDto item)
  {
    ViewBag.UserName = _httpContextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    try
    {
      var success = await _itemService.UpdateItemAsync(id, item);
      SetTempMessage(success, "Item updated successfully", "Failed");
      return RedirectToAction("Index", "Collections");
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = $"An error occurred while updating the item: {ex.Message}";
      // Log the exception
      return RedirectToAction("Index", "Collections"); // Redirect to the home page with an error message
    }
  }

  [Authorize]
  [HttpGet("likeitem")]
  public async Task<IActionResult> LikeItem(int id, int itemId)
  {
    ViewBag.CollectionId = id;
    try
    {
      var res = await _likeService.ToggleItem(itemId);
      if (res)
      {
        return RedirectToAction("Index", "Items", new { id });
      }
      else
      {
        TempData["Error"] = "Failed to like item";
        return RedirectToAction("Index", "Items", new { id });
      }
    }
    catch (Exception ex)
    {
      TempData["Error"] = ex.Message;
      return RedirectToAction("Index", "Items", new { id });
    }
  }
  private void SetTempMessage(bool success, string successMessage, string errorMessage)
  {
    TempData[success ? "SuccessMessage" : "ErrorMessage"] = success ? successMessage : errorMessage;
  }
}

