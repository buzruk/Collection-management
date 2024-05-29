using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services;

public class UserService(UserManager<User> userManager,
                         IConfiguration configuration,
                         IWebHostEnvironment webHostEnvironment,
                         IOneTimePasswordService oneTimePasswordService,
                         IImageService imageService)
    : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly IOneTimePasswordService _oneTimePasswordService = oneTimePasswordService;
    private readonly IImageService _imageService = imageService;

    /// <summary>
    /// Change user password using user manager
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="CollectionException"></exception>
    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var user = await _userManager.FindByNameAsync(dto.UserName);

        if (user is null)
            throw new ArgumentNullException("User not found");

        var result = await _userManager.ChangePasswordAsync(user,
                                                            dto.OldPassword,
                                                            dto.NewPassword);
        if (!result.Succeeded)
            throw new CollectionException("Failed to change password");
    }

    /// <summary>
    /// Deletes a user account using user manager
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task RemoveAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            throw new ArgumentException("User not found");

        //var roles = await _userManager.GetRolesAsync(user);
        //await _userManager.RemoveFromRoleAsync(user, roles[0]);
        var provider = _configuration["Jwt:Issuer"] ?? "";
        await _userManager.RemoveAuthenticationTokenAsync(user, provider, "Token");
        await DeleteProfileImageAsync(id);

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new CollectionException("Failed to delete user");
    }

    /// <summary>
    /// Deletes a user accounts using user manager
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public async Task RemoveAsync(List<string> ids)
    {
        foreach (string id in ids)
        {
            await RemoveAsync(id);
        }
    }

    /// <summary>
    /// Set profile picture using image service
    /// </summary>
    /// <param name="file"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task SetProfileImageAsync(SetAvatarDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException("User not found");

        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            throw new ArgumentNullException("User not found");

        user.Image = dto.ImageUrl;
        await _userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Update profile picture using image service
    /// </summary>
    /// <param name="file"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task UpdateProfileImageAsync(SetAvatarDto dto)
    {
        var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        var domain = _configuration["Domain"] ?? "";
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            throw new ArgumentNullException("User not found");

        if (user == null)
            throw new StatusCodeException(HttpStatusCode.NotFound, "user is not found");


        if (user.Image != null)
            await _imageService.RemoveAsync(user.Image, folder);

        //user.Image = await _imageService.SaveImageAsync(file);
        user.Image = dto.ImageUrl;
        await _userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Delete profile picture using image service
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task DeleteProfileImageAsync(string id)
    {
        var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new ArgumentNullException("User not found");

        await _imageService.RemoveAsync(user.Image, folder);
        user.Image = "";
        await _userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Get user by id using user manager
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<UserDto> GetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new ArgumentNullException("User not found");

        return (UserDto)user;
    }

    /// <summary>
    /// Get users by role using user manager
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>

    //public async Task<List<UserDto>> GetUsersAsync(string role)
    //{
    //  var users = await _userManager.GetUsersInRoleAsync(role);
    //  return users.Select(x => (UserDto)x).ToList();
    //}

    public async Task<PagedResults<UserViewModel>> GetPagedAsync(string role, PaginationParams @params, CancellationToken cancellationToken = default)
    {
        var users = await _userManager.GetUsersInRoleAsync(role);

        if (users == null)
            throw new CollectionException("Users not found");

        return users.OrderBy(u => u.Id)
             .Select(x => (UserViewModel)x)
             .ToPagedResult(@params.PageSize, @params.PageNumber);
    }


    /// <summary>
    /// Update user using user manager
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FurnitureException"></exception>
    public async Task UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var user = await _userManager.FindByIdAsync(dto.Id.ToString());

        if (user is null)
            throw new ArgumentNullException("User not found");
        //throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");

        user.UserName = string.IsNullOrEmpty(dto.UserName) ? user.UserName : dto.UserName;
        user.BirthDate = dto.BirthDate;
        user.Image = string.IsNullOrEmpty(dto.ImagePath) ? user.Image : dto.ImagePath;

        if (dto.Image != null)
        {
            user.Image = await _imageService.UploadAsync(dto.Image, "", "");
        }

        user.UpdatedAt = TimeHelper.GetCurrentServerTime();

        if (user.UserName != dto.UserName)
        {
            var exsistingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (exsistingUser is not null)
            {
                throw new CollectionException("username already exists");
            }
            else
            {
                user.UserName = dto.UserName;
                user.PhoneNumberConfirmed = false;
                await _userManager.RemoveAuthenticationTokenAsync(user, _configuration["Jwt:Issuer"] ?? "", "Token");
                await _userManager.SetUserNameAsync(user, dto.UserName);
            }
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new CollectionException("Failed to update user");
    }

    /// <summary>
    /// Change user phone number using user manager
    /// </summary>
    /// <param name="id"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FurnitureException"></exception>
    public async Task ChangeEmail(string id, string email)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new ArgumentNullException("User not found");

        user.Email = email;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new CollectionException("Failed to update user");
    }

    public async Task BlockAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new ArgumentNullException("User not found");

        user.Status = StatusType.Blocked;
        await _userManager.UpdateAsync(user);
    }

    public async Task BlockAsync(List<string> ids)
    {
        foreach (string id in ids)
        {
            await BlockAsync(id);
        }
    }

    public async Task ActiveAsync(List<string> ids)
    {
        foreach (var id in ids)
        {
            await ActiveAsync(id);
        }
    }

    public async Task ActiveAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new ArgumentNullException("User not found");

        user.Status = StatusType.Active;
        await _userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Reset user password using user manager and send the new password via SMS
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FurnitureException"></exception>
    public async Task ResetPassword(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            throw new ArgumentNullException("User not found");

        //user.PasswordHash = dto.NewPassword;
        await _userManager.RemoveAuthenticationTokenAsync(user, _configuration["Jwt:Issuer"] ?? "", "Token");
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, dto.NewPassword);
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new CollectionException("Failed to update user");

        string message = $"""
        Your account password has been reset.
        Email: {user.Email}
        Password: {dto.NewPassword}
        """;

        var sendOtpDto = new SendOtpDto()
        {
            Email = user.Email!,
            Subject = "Password reset result",
            Message = message
        };
        _oneTimePasswordService.SendEmail(sendOtpDto);
    }
}
