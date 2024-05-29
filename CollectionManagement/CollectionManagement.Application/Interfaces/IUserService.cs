namespace CollectionManagement.Application.Interfaces;

public interface IUserService
{
    // Task<PagedList<UserViewModel>> GetAllAysnc(PaginationParams @params, CancellationToken cancellationToken = default);
    // Task<PagedList<UserViewModel>> SearchAsync(PaginationParams @params, string name, CancellationToken cancellationToken = default);
    // Task<UserRankViewModel> GetRankAsync(int id, CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(ChangePasswordDto dto);

    Task RemoveAsync(string id);

    Task RemoveAsync(List<string> ids);

    Task SetProfileImageAsync(SetAvatarDto dto);

    Task UpdateProfileImageAsync(SetAvatarDto dto);

    Task DeleteProfileImageAsync(string id);

    Task<UserDto> GetAsync(string id);

    Task<PagedResults<UserViewModel>> GetPagedAsync(string role, PaginationParams @params, CancellationToken cancellationToken = default);

    Task UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken = default);

    Task ChangeEmail(string id, string email);

    Task BlockAsync(string id);

    Task BlockAsync(List<string> ids);

    Task ActiveAsync(List<string> ids);

    Task ActiveAsync(string id);

    Task ResetPassword(ResetPasswordDto dto);
}

