namespace CollectionManagement.Application.Interfaces;

public interface IItemService
{
    Task<PagedResults<ItemViewModel>> GetPagedAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default);

    // Task<List<LikePerItemViewModel>> GetAllLikeByItemAsync(int collectionId, CancellationToken cancellationToken = default);

    Task<bool> AddAsync(ItemDto item, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, ItemUpdateDto dto, CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);
}

