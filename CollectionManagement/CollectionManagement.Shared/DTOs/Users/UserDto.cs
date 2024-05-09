namespace CollectionManagement.Shared.DTOs.Users;

public class UserDto : BaseDto
{
  public string UserName { get; set; } = String.Empty;

  public string ImagePath { get; set; } = String.Empty;

  public List<Collection>? Collections { get; set; }

  public DateTime BirthDate { get; set; }

  public DateTime CreatedAt { get; set; }

  public FileDto? FileModel { get; set; }

  public static implicit operator UserDto(User user)
  {
    return new UserDto()
    {
      Id = user.Id,
      UserName = user.UserName,
      ImagePath = user.Image!,
      BirthDate = user.BirthDate,
      CreatedAt = user.CreatedAt,
    };
  }
}

