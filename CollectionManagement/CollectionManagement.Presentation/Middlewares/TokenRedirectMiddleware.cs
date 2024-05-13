namespace CollectionManagement.Presentation.Middlewares;

public class TokenRedirectMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public Task InvokeAsync(HttpContext httpContext)
  {
    if (httpContext.Request.Cookies.TryGetValue("X-Access-Token", out var accessToken))
    {
      if (!string.IsNullOrEmpty(accessToken))
      {
        string bearerToken = String.Format("Bearer {0}", accessToken);
        httpContext.Request.Headers.Add("Authorization", bearerToken);
      }
    }
    return _next(httpContext);
  }
}

