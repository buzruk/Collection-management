namespace CollectionManagement.Presentation.Controllers;

[Route("tags")]
public class TagsController(ITagService tagService)
  : Controller
{
    private readonly ITagService _tagService = tagService;

    [HttpPost("create")]
    public async Task<IActionResult> Create(IEnumerable<string> tags, Item item)
    {
        try
        {
            await _tagService.AddAsync(tags, item);
            return RedirectToAction("Index", "Collection");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Collection");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(IEnumerable<string> tags, Item item)
    {
        try
        {
            await _tagService.UpdateAsync(tags, item);
            return RedirectToAction("Index", "Collection");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Collection");
        }
    }
}

