namespace CollectionManagement.Infrastructure.Interfaces.Items;

public interface IItemService
{
  Task<PagedResults<ItemViewModel>> GetPagedAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default);

  // Task<List<LikePerItemViewModel>> GetAllLikeByItemAsync(int collectionId, CancellationToken cancellationToken = default);

  Task<bool> CreateItemAsync(ItemDto item, CancellationToken cancellationToken = default);

  Task<bool> UpdateItemAsync(int id, ItemUpdateDto item, CancellationToken cancellationToken = default);

  Task<bool> DeleteItemAsync(int id, CancellationToken cancellationToken = default);
}

