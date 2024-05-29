namespace CollectionManagement.Presentation.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
  private RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (StatusCodeException exception)
    {
      await UserErrorHandlerAsync(exception, context);
    }
    catch (Exception ex)
    {
      await ServiceErrorHandlerAsync(ex, context);
    }
  }

  public async Task UserErrorHandlerAsync(StatusCodeException exception, HttpContext context)
  {
    context.Response.ContentType = "application/json";
    var dto = new ErrorDto()
    {
      StatusCode = (int)exception.StatusCode,
      Message = exception.Message
    };
    string jsonData = JsonSerializer.Serialize(dto);
    context.Response.StatusCode = (int)exception.StatusCode;
    await context.Response.WriteAsync(jsonData);
  }
  public async Task ServiceErrorHandlerAsync(Exception exception, HttpContext context)
  {
    ErrorDto dto = new()
    {
      Message = exception.Message,
      StatusCode = 500
    };
    string jsonData = JsonSerializer.Serialize(dto);
    context.Response.StatusCode = 500;
    await context.Response.WriteAsync(jsonData);
  }
}

