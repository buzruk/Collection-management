namespace CollectionManagement.Application.Services.Common;

public class PaginatorService(IHttpContextAccessor httpContextAccessor) 
  : IPaginatorService
{
  private readonly IHttpContextAccessor _accessor = httpContextAccessor;

  public IEnumerable<T> ToPaged<T>(IEnumerable<T> items, 
                                   int pageNumber, 
                                   int pageSize,
                                   CancellationToken cancellationToken = default)
  {
    var pagedResult = items.ToPagedResult(pageSize, pageNumber);

    string json = JsonSerializer.Serialize(pagedResult);

    //_accessor.HttpContext.Response.Headers.Add("X-Pagination", json);
    _accessor.HttpContext!.Response.Headers.Append("X-Pagination", json);

    return pagedResult.GetPage();
  }
}

