namespace CollectionManagement.Infrastructure.Interfaces.Collections;

public interface ICollectionService
{
  // Task<PagedList<CollectionViewModel>> SearchAsync(PaginationParams @params, string name);
  Task<PagedResults<CollectionViewModel>> SearchAsync(CancellationToken cancellationToken = default);

  // Task<PagedList<CollectionViewModel>> TopCollection(PaginationParams @params);
  Task<PagedResults<CollectionViewModel>> TopCollection(CancellationToken cancellationToken = default);

  // Task<PagedList<CollectionViewModel>> GetAllCollectionAsync(PaginationParams @params);
  Task<PagedResults<CollectionViewModel>> GetAllCollectionAsync(CancellationToken cancellationToken = default);

  // Task<List<LikePerCollectionViewModel>> GetAllLikeByCollectionAsync(int collectionId, CancellationToken cancellationToken = default);

  Task<bool> CreateCollectionAsync(CollectionDto collectionCreateDto, CancellationToken cancellationToken = default);

  Task<bool> DeleteCollectionAsync(int id, CancellationToken cancellationToken = default);

  Task<bool> UpdateCollectionAsync(int id, CollectionUpdateDto collectionUpdateDto, CancellationToken cancellationToken = default);

  Task<bool> GetCollectionById(int userId, int collectionId, CancellationToken cancellationToken = default);
}

