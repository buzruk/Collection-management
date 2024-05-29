namespace CollectionManagement.Application.Services.Common;

public class IdentityService(IHttpContextAccessor httpContextAccessor)
  : IIdentityService
{
  private readonly IHttpContextAccessor _accessor = httpContextAccessor;

  public string Id
  {
    get
    {
      //var res = _accessor.HttpContext!.User.FindFirst("Id");
      //return res is not null ? res.Value : "";
      var user = _accessor.HttpContext?.User;
      if (user !=  null && user.Identity!.IsAuthenticated)
      {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim != null)
        {
          //collection.UserId = userIdClaim.Value;
          return userIdClaim.Value;
        }
      }
      return "";
    }
  }

  public string UserName
  {
    get
    {
      var result = _accessor.HttpContext!.User.FindFirst("UserName");
      return (result is null) ? string.Empty : result.Value;
      //var user = _accessor.HttpContext?.User;
      //if (user !=  null && user.Identity!.IsAuthenticated)
      //{
      //  var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
      //  if (userIdClaim != null)
      //  {
      //    //collection.UserId = userIdClaim.Value;
      //    return userIdClaim.Value;
      //  }
      //}
      //return "";
    }
  }

  public string ImagePath
  {
    get
    {
      var result = _accessor.HttpContext!.User.FindFirst("Image");
      return (result is null) ? string.Empty : result.Value;
    }
  }
}

