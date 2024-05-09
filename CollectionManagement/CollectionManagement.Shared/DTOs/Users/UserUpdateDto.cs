namespace CollectionManagement.Shared.DTOs.Users;

public class UserUpdateDto
{
  [Required, MaxLength(30), MinLength(3)]
  public string UserName { get; set; } = string.Empty;

  public DateTime BirthDate { get; set; }

  public IFormFile? Image { get; set; }

  public string ImagePath { get; set; } = string.Empty;
}

