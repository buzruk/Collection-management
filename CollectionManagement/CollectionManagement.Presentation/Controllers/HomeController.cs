namespace CollectionManagement.Presentation.Controllers;

public class HomeController(ICollectionService collectionService,
                            IHttpContextAccessor httpContextAccessor) 
  : Controller
{
  private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
  private readonly ICollectionService _collectionService = collectionService;

  public async Task<IActionResult> Index()
  {
    ViewBag.UserName = _contextAccessor.HttpContext?.User.FindFirst("UserName")?.Value;
    var result = await _collectionService.TopCollection(new PaginationParams(1, 4));
    return View(result);
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}

