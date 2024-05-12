namespace CollectionManagement.Infrastructure.Interfaces.Common;

public interface IPaginatorService
{
  Task<IEnumerable<T>> ToPagedAsync<T>(IEnumerable<T> items,
                                                    int pageNumber,
                                                    int pageSize,
                                                    CancellationToken cancellationToken = default);
}

