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

    // Task<PagedList<CollectionViewModel>> GetAllCollectionAsync(PaginationParams @params);
    Task<PagedResults<CollectionViewModel>> GetAllCollectionAsync(CancellationToken cancellationToken = default);

    // Task<PagedList<ItemViewModel>> GetAllItemAsync(int id, PaginationParams @params);
    Task<PagedResults<ItemViewModel>> GetAllItemAsync(CancellationToken cancellationToken = default);

    // Task<PagedList<UserViewModel>> GetAllAsync(PaginationParams @params);
    Task<PagedResults<UserViewModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CollectionViewModel> GetCollectionById(int id, CancellationToken cancellationToken = default);

    Task<ItemViewModel> GetItemById(int id, CancellationToken cancellationToken = default);

    // Task<PagedList<CommentViewModel>> GetAllComments(int id, PaginationParams @params);
    Task<PagedResults<CommentViewModel>> GetAllComments(CancellationToken cancellationToken = default);
}

