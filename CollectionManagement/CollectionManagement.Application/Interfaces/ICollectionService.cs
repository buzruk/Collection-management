namespace CollectionManagement.Application.Interfaces;

public interface ICollectionService
{
    Task<PagedResults<CollectionViewModel>> SearchAsync(PaginationParams @params, string name, CancellationToken cancellationToken = default);

    Task<PagedResults<CollectionViewModel>> TopCollection(PaginationParams @params, CancellationToken cancellationToken = default);

    Task<PagedResults<CollectionViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default);

    // Task<List<LikePerCollectionViewModel>> GetAllLikeByCollectionAsync(int collectionId, CancellationToken cancellationToken = default);

    Task<bool> AddAsync(CollectionDto collectionCreateDto, CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, CollectionUpdateDto collectionUpdateDto, CancellationToken cancellationToken = default);

    Task<CollectionViewModel> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

