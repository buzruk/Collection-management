namespace CollectionManagement.Application.Interfaces.Common;

public interface IPaginatorService
{
  IEnumerable<T> ToPaged<T>(IEnumerable<T> items,
                                       int pageNumber,
                                       int pageSize,
                                       CancellationToken cancellationToken = default);
}

