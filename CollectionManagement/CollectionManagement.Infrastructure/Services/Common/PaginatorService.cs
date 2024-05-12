namespace CollectionManagement.Infrastructure.Services.Common;

public class PaginatorService(IHttpContextAccessor httpContextAccessor) 
  : IPaginatorService
{
  private readonly IHttpContextAccessor _accessor = httpContextAccessor;

  public async Task<IEnumerable<T>> ToPagedAsync<T>(IEnumerable<T> items, 
                                                    int pageNumber, 
                                                    int pageSize,
                                                    CancellationToken cancellationToken = default)
  {
    var pagedResult = items.ToPagedResult(pageSize, pageNumber);

    //string json = await JsonSerializer.SerializeAsync(pagedResult);
    string json = JsonSerializer.Serialize(pagedResult);

    _accessor.HttpContext!.Response.Headers.Add("X-Pagination", json);

    return pagedResult.GetPage();
  }
}

