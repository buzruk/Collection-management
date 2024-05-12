namespace CollectionManagement.Infrastructure.Interfaces.Admins;

public interface IAdminUserService
{
  Task<bool> BlockAsync(List<int> ids, CancellationToken cancellationToken = default);

  Task<bool> ActiveAsync(List<int> ids, CancellationToken cancellationToken = default);

  Task<bool> DeleteAsync(List<int> ids, CancellationToken cancellationToken = default);

  //Task<List<UserViewModel>> GetAllAsync(string search, CancellationToken cancellationToken = default);

  Task<PagedResults<UserViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default);

  //Task<PagedList<UserViewModel>> GetByNameAsync(PaginationParams @params, string name, CancellationToken cancellationToken = default);

  Task<bool> UpdateAsync(int id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);

  //Task<bool> UpdateImageAsync(int id, IFormFile from, CancellationToken cancellationToken = default);

  Task<bool> DeleteImageAsync(int adminId, CancellationToken cancellationToken = default);

  //Task<bool> UpdatePasswordAsync(int id, PasswordUpdateDto dto, CancellationToken cancellationToken = default);
}

