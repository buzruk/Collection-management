namespace CollectionManagement.Infrastructure.Services.Common;

public class IdentityService(IHttpContextAccessor accessor) 
  : IIdentityService
{
  private readonly IHttpContextAccessor _accessor = accessor;

  public int? Id
  {
    get
    {
      var res = _accessor.HttpContext!.User.FindFirst("Id");
      return res is not null ? int.Parse(res.Value) : null;
    }
  }

  public string UserName
  {
    get
    {
      var result = _accessor.HttpContext!.User.FindFirst("UserName");
      return (result is null) ? String.Empty : result.Value;
    }
  }

  public string ImagePath
  {
    get
    {
      var result = _accessor.HttpContext!.User.FindFirst("Image");
      return (result is null) ? String.Empty : result.Value;
    }
  }
}

