namespace CollectionManagement.Infrastructure.Interfaces.Collections;

public interface ICollectionService
{
  Task<PagedResults<CollectionViewModel>> SearchAsync(PaginationParams @params, string name, CancellationToken cancellationToken = default);

  Task<PagedResults<CollectionViewModel>> TopCollection(PaginationParams @params, CancellationToken cancellationToken = default);

  Task<PagedResults<CollectionViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default);

  // Task<List<LikePerCollectionViewModel>> GetAllLikeByCollectionAsync(int collectionId, CancellationToken cancellationToken = default);

  Task<bool> CreateCollectionAsync(CollectionDto collectionCreateDto, CancellationToken cancellationToken = default);

  Task<bool> DeleteCollectionAsync(int id, CancellationToken cancellationToken = default);

  Task<bool> UpdateCollectionAsync(int id, CollectionUpdateDto collectionUpdateDto, CancellationToken cancellationToken = default);

  Task<bool> GetCollectionById(int userId, int collectionId, CancellationToken cancellationToken = default);
}

