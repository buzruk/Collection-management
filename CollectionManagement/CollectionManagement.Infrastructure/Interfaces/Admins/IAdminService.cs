namespace CollectionManagement.Infrastructure.Interfaces.Admins;

public interface IAdminService
{
  Task<bool> DeleteAsync(List<int> ids, CancellationToken cancellationToken = default);

  Task<bool> BlockAsync(List<int> ids, CancellationToken cancellationToken = default);

  Task<bool> ActiveAsync(List<int> ids, CancellationToken cancellationToken = default);

  Task<List<AdminViewModel>> GetAllAsync(string search, CancellationToken cancellationToken = default);

  //Task<PagedList<AdminViewModel>> GetAllAsync(PaginationParams @params);
  // TODO: Fix it
  Task<PagedResults<AdminViewModel>> GetPagedAsync(CancellationToken cancellationToken = default);

  //Task<PagedList<AdminViewModel>> GetByNameAsync(PaginationParams @params, string name);
  Task<bool> UpdateAsync(int id, AdminUpdateDto adminUpdateDto, CancellationToken cancellationToken = default);

  Task<bool> UpdateImageAsync(int id, IFormFile from, CancellationToken cancellationToken = default);

  Task<bool> DeleteImageAsync(int adminId, CancellationToken cancellationToken = default);

  Task<bool> UpdatePasswordAsync(int id, PasswordUpdateDto dto, CancellationToken cancellationToken = default);

  Task<bool> CreateAdminAsync(AdminRegisterDto adminCreateDto, CancellationToken cancellationToken = default);
}

