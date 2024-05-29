using CollectionManagement.Shared.DTOs.Tags;

namespace CollectionManagement.Presentation.Controllers;

public class TagsController(ITagService tagService)
  : Controller
{
    private readonly ITagService _tagService = tagService;

    [HttpPost]
    public async Task<IActionResult> Create(AddTagDto request)
    {
        try
        {
            await _tagService.AddAsync(request);
            return RedirectToAction("Index", "Collection");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Collection");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTagDto request)
    {
        try
        {
            await _tagService.UpdateAsync(request);
            return RedirectToAction("Index", "Collection");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Collection");
        }
    }
}

