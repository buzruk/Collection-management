namespace CollectionManagement.Shared.ViewModels.UserViewModels;

public class UserViewModel
{
  public int Id { get; set; }

  public string UserName { get; set; } = string.Empty;

  public string ImagePath { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public StatusType StatusType { get; set; } = StatusType.Active;

  public string UserRole { get; set; } = RoleConstants.User;

  public DateTime BirthDate { get; set; } = default!;

  public DateTime CreatedAt { get; set; } = default!;

  //public static implicit operator UserViewModel(User model)
  //{
  //  return new UserViewModel()
  //  {
  //    Id = model.Id,
  //    UserName = model.UserName,
  //    ImagePath = model.Image,
  //    Email = model.Email,
  //    StatusType = model.Status,
  //    UserRole = model.UserRole,
  //    BirthDate = model.BirthDate,
  //    CreatedAt = model.CreatedAt
  //  };
  //}
}

