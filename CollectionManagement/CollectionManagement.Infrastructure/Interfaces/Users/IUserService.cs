namespace CollectionManagement.Infrastructure.Interfaces.Users;

public interface IUserService
{
  // Task<PagedList<UserViewModel>> GetAllAysnc(PaginationParams @params, CancellationToken cancellationToken = default);
  // Task<PagedList<UserViewModel>> SearchAsync(PaginationParams @params, string name, CancellationToken cancellationToken = default);
  // Task<UserRankViewModel> GetRankAsync(int id, CancellationToken cancellationToken = default);

  Task<bool> UpdateAsync(int id, UserUpdateDto entity, CancellationToken cancellationToken = default);

  Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

  Task<bool> DeleteImageAsync(int id, CancellationToken cancellationToken = default);

  Task<bool> ImageUpdateAsync(int id, IFormFile file, CancellationToken cancellationToken = default);

  Task<bool> UpdatePasswordAsync(int id, PasswordUpdateDto dto, CancellationToken cancellationToken = default);

  Task<PagedResults<CollectionViewModel>> GetPagedCollectionsAsync(PaginationParams @params, CancellationToken cancellationToken = default);

  Task<PagedResults<ItemViewModel>> GetPagedItemsAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default);

  Task<PagedResults<UserViewModel>> GetPagedUsersAsync(PaginationParams @params, CancellationToken cancellationToken = default);

  Task<ItemViewModel> GetItemByIdAsync(int id, CancellationToken cancellationToken = default);

  Task<PagedResults<CommentViewModel>> GetPagedCommentsAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default);

  Task<CollectionViewModel> GetCollectionByIdAsync(int id, CancellationToken cancellationToken = default);
}

