using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Shared.ViewModels.UserViewModels;

public class UserViewModel
{
  public string Id { get; set; } = string.Empty;

  public string UserName { get; set; } = string.Empty;

  public string ImagePath { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public StatusType StatusType { get; set; } = StatusType.Active;

  public string Role { get; set; } = RoleConstants.User;

  public DateTime BirthDate { get; set; } = default!;

  public DateTime CreatedAt { get; set; } = default!;

  public static explicit operator UserViewModel(User model)
  {
    return new UserViewModel()
    {
      Id = model.Id,
      UserName = model.UserName!,
      ImagePath = model.Image,
      Email = model.Email!,
      StatusType = model.Status,
      Role = model.Role,
      BirthDate = model.BirthDate,
      CreatedAt = model.CreatedAt
    };
  }

  public static explicit operator User(UserViewModel model)
  {
    return new User()
    {
      Id = model.Id,
      UserName = model.UserName,
      Image = model.ImagePath,
      Email = model.Email,
      Status = model.StatusType,
      Role = model.Role,
      BirthDate = model.BirthDate,
      CreatedAt = model.CreatedAt
    };
  }
}

