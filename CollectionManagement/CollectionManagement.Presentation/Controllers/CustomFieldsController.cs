namespace CollectionManagement.Presentation.Controllers;

public class CustomFieldsController(ICustomFieldService customFieldService)
  : Controller
{
    private readonly ICustomFieldService _customFieldService = customFieldService;

    [HttpPost("create")]
    public async Task<IActionResult> Create(int id, CustomFieldDto customFieldDto)
    {
        try
        {
            var success = await _customFieldService.AddAsync(id, customFieldDto);
            SetTempMessage(success, "CustomField created successfully", "Failed");
            return View(success);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred while creating the custom field: {ex.Message}";
            // Log the exception
            return RedirectToAction("Index", "Home"); // Redirect to the home page with an error message
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _customFieldService.RemoveAsync(id);
            SetTempMessage(success, "CustomField deleted successfully", "Failed");
            return View(success);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred while deleting the custom field: {ex.Message}";
            // Log the exception
            return RedirectToAction("Index", "Home"); // Redirect to the home page with an error message
        }
    }
    private void SetTempMessage(bool success, string successMessage, string errorMessage)
    {
        TempData[success ? "SuccessMessage" : "ErrorMessage"] = success ? successMessage : errorMessage;
    }
}

