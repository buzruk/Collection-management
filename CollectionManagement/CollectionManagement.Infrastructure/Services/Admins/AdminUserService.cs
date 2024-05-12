namespace CollectionManagement.Infrastructure.Services.Admins;

public class AdminUserService(IUnitOfWorkAsync unitOfWork,
                              IAuthService authService,
                              IImageService imageService,
                              IFileService fileService)
  : IAdminUserService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
  private readonly IAuthService _authService = authService;
  private readonly IImageService _imageService = imageService;
  private readonly IFileService _fileService = fileService;

  public async Task<bool> DeleteAsync(List<int> ids, CancellationToken cancellationToken = default)
  {
    foreach (var id in ids)
    {
      var userRepostiory = await _unitOfWork.GetRepositoryAsync<User>();
      var user = await userRepostiory.GetAsync(u => u.Id == id);

      if (user is not null)
      {
        await userRepostiory.RemoveAsync(u => u.Id == id);
      }
    }
    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> BlockAsync(List<int> ids, CancellationToken cancellationToken = default)
  {
    foreach (var id in ids)
    {
      var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
      var user = await userRepository.GetAsync(u => u.Id == id);

      if (user is not null)
      {
        if (user.Status != StatusType.Blocked)
        {
          user.Status = StatusType.Blocked;
          user.Id = id;
          await userRepository.UpdateAsync(user);
        }
      }
    }
    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> ActiveAsync(List<int> ids, CancellationToken cancellationToken = default)
  {
    foreach (var id in ids)
    {
      var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
      var user = await userRepository.GetAsync(u => u.Id == id);

      if (user is not null)
      {
        if (user.Status != StatusType.Active)
        {
          user.Status = StatusType.Active;
          user.Id = id;
          await userRepository.UpdateAsync(user);
        }
      }
    }

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> DeleteImageAsync(int userId, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(u => u.Id == userId);

    if (user is null) throw new NotFoundException("Admin", $"{userId} not found");
    else
    {
      await _fileService.DeleteImageAsync(user.Image!);
      user.Image = "";
      user.Id = userId;
      await userRepository.UpdateAsync(user);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
  }

  //public async Task<List<UserViewModel>> GetAllAsync(string search, CancellationToken cancellationToken = default)
  //{
  //  var query = _unitOfWork.Users.GetAll();
  //  if (!string.IsNullOrEmpty(search))
  //  {
  //    query = query.Where(x => x.UserName.ToLower().StartsWith(search.ToLower()));
  //  }

  //  var result = await query.OrderByDescending(x => x.CreatedAt).Select(x => (UserViewModel)x).ToListAsync();
  //  return result;
  //}

  public async Task<PagedResults<UserViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var pagedResult = await adminRepository.GetPagedAsync(orderBy: a => a.Id, 
                                                          pageNumber: @params.PageNumber,
                                                          pageSize: @params.PageSize);
    var result = new PagedResults<UserViewModel>()
    {
      Items = pagedResult.Items.Select(a => (UserViewModel)a),
      TotalItemsCount = pagedResult.TotalItemsCount,
      TotalPages = pagedResult.TotalPages,
      PageSize = pagedResult.PageSize,
      PageNumber = pagedResult.PageNumber,
      HasPreviousPage = pagedResult.PageNumber > 1,
      HasNextPage = pagedResult.PageNumber < pagedResult.TotalPages,
      IsFirstPage = pagedResult.PageNumber == 1,
      IsLastPage = pagedResult.PageNumber == pagedResult.TotalPages,
      FirstItemOnPage = (pagedResult.PageNumber - 1) * pagedResult.PageSize + 1,
      LastItemOnPage = Math.Min(pagedResult.PageNumber * pagedResult.PageSize, pagedResult.TotalItemsCount)
    };
    return result;
  }

  public async Task<bool> UpdateAsync(int id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(u => u.Id == id);

    if (user is null) throw new NotFoundException("User", $"{id} not found");

    //_unitOfWork.Users.TrackingDeteched(user);
    // TrackingDeteched --> _dbContext.Entry<T>(entity!).State = EntityState.Detached;

    if (userUpdateDto != null)
    {
      user.UserName = string.IsNullOrEmpty(userUpdateDto.UserName) ? user.UserName : userUpdateDto.UserName;

      user.Image = string.IsNullOrEmpty(userUpdateDto.ImagePath) ? user.Image : userUpdateDto.ImagePath;

      user.BirthDate = user.BirthDate;

      if (userUpdateDto.Image is not null)
      {
        user.Image = await _fileService.UploadImageAsync(userUpdateDto.Image);
      }

      user.UpdatedAt = TimeHelper.GetCurrentServerTime();
      user.Id = id;
      await userRepository.UpdateAsync(user);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else throw new ModelErrorException("", "Not found");
  }
}

