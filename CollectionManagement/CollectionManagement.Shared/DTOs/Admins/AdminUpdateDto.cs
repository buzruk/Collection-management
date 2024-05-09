namespace CollectionManagement.Shared.DTOs.Admins;

public class AdminUpdateDto : BaseDto
{
  [Required(ErrorMessage = "UserName Required")]
  public string UserName { get; set; } = String.Empty;

  public IFormFile Image { get; set; } = default!;

  public string ImagePath { get; set; } = String.Empty;

  public string Address { get; set; } = String.Empty;

  public DateTime BirthDate { get; set; }

  public static implicit operator Admin(AdminUpdateDto dto)
  {
    return new Admin()
    {
      UserName = dto.UserName,
      Image = dto.ImagePath,
      BirthDate = dto.BirthDate,
      Address = dto.Address
    };
  }
}

