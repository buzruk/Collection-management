namespace CollectionManagement.Shared.DTOs.Files;

public class FileDto
{
  [AllowedFiles([".xlsx"])]
  [Required]
  public IFormFile? File { get; set; }
}

