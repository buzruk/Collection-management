using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Shared.DTOs.Users;

public class UserDto 
{
  public string Id { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;

  public string ImagePath { get; set; } = string.Empty;

  public List<Collection>? Collections { get; set; }

  public DateTime BirthDate { get; set; }

  public DateTime CreatedAt { get; set; }

  public FileDto? FileModel { get; set; }

  public string UserId { get; set; } = string.Empty;

  public string? Token { get; set; }

  public List<string> Roles { get; set; } = [];

  public static implicit operator UserDto(User user)
  {
    return new UserDto()
    {
      Id = user.Id,
      UserName = user.UserName!,
      ImagePath = user.Image!,
      BirthDate = user.BirthDate,
      CreatedAt = user.CreatedAt,
    };
  }
}

