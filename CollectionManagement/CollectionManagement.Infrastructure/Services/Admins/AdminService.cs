namespace CollectionManagement.Infrastructure.Services.Admins;

public class AdminService(IUnitOfWorkAsync unitOfWork,
                          IIdentityService identityService,
                          IFileService fileService)
  : IAdminService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
  private readonly IIdentityService _identityService = identityService;
  private readonly IFileService _fileService = fileService;

  public async Task<bool> DeleteAsync(List<int> ids, CancellationToken cancellationToken = default)
  {
    foreach (var id in ids)
    {
      var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
      var temp = await adminRepository.GetAsync(a => a.Id == id);

      if (temp is not null)
      {
        await adminRepository.RemoveAsync(a => a.Id == id);
      }
    }
    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> BlockAsync(List<int> ids, CancellationToken cancellationToken = default)
  {
    foreach (var id in ids)
    {
      var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
      var temp = await adminRepository.GetAsync(a => a.Id == id);

      if (temp != null)
      {
        if (temp.Status != StatusType.Blocked)
        {
          temp.Status = StatusType.Blocked;
          temp.Id = id;
          await adminRepository.UpdateAsync(temp);
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
      var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
      var temp = await adminRepository.GetAsync(a => a.Id == id);

      if (temp != null)
      {
        if (temp.Status != StatusType.Active)
        {
          temp.Status = StatusType.Active;
          temp.Id = id;
          await adminRepository.UpdateAsync(temp);
        }
      }
    }
    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }
  public async Task<bool> DeleteImageAsync(int adminId, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var admin = await adminRepository.GetAsync(a => a.Id == adminId);

    if (admin is null) throw new NotFoundException("Admin", $"{adminId} not found");
    else
    {
      await _fileService.DeleteImageAsync(admin.Image!);
      admin.Image = "";
      admin.Id = adminId;
      await adminRepository.UpdateAsync(admin);
      var res = await _unitOfWork.SaveChangesAsync();
      return res > 0;
    }
  }
  public async Task<List<AdminViewModel>> GetAllAsync(string search, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    IEnumerable<Admin> query;

    if (!string.IsNullOrEmpty(search))
    {
      query = await adminRepository.GetAllAsync(predicates: [
          x => x.UserName.ToLower().StartsWith(search.ToLower())
          ||
          x.Address.ToLower().StartsWith(search.ToLower())
        ]);
    }
    else
    {
      query = await adminRepository.GetAllAsync();
    }

    var result = query.OrderByDescending(x => x.CreatedAt)
                            .Select(x => (AdminViewModel)x)
                            .ToList();
    return result;
  }

  public async Task<PagedResults<AdminViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var pagedResult = await adminRepository.GetPagedAsync(orderBy: a => a.Id, 
                                                          pageNumber: @params.PageNumber,
                                                          pageSize: @params.PageSize);
    var result = new PagedResults<AdminViewModel>()
    {
      Items = pagedResult.Items.Select(a => (AdminViewModel)a),
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

  public async Task<bool> UpdateAsync(int id, AdminUpdateDto adminUpdateDto, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var admin = await adminRepository.GetAsync(a => a.Id == id);

    if (admin is null) throw new NotFoundException("Admin", $"{id} not found");

    // _unitOfWork.Admins.TrackingDeteched(admin);
    // TrackingDeteched --> _dbContext.Entry<T>(entity!).State = EntityState.Detached;

    if (adminUpdateDto is not null)
    {
      admin.UserName = string.IsNullOrEmpty(adminUpdateDto.UserName) ? admin.UserName : adminUpdateDto.UserName;

      admin.Image = string.IsNullOrEmpty(adminUpdateDto.ImagePath) ? admin.Image : adminUpdateDto.ImagePath;

      admin.BirthDate = admin.BirthDate;

      admin.Address = string.IsNullOrEmpty(adminUpdateDto.Address) ? admin.Address : adminUpdateDto.Address;

      if (adminUpdateDto.Image is not null)
      {
        admin.Image = await _fileService.UploadImageAsync(adminUpdateDto.Image);
      }

      admin.UpdatedAt = TimeHelper.GetCurrentServerTime();
      admin.Id = id;
      await adminRepository.UpdateAsync(admin);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else throw new ModelErrorException("", "Not found");
  }

  public async Task<bool> UpdateImageAsync(int id, IFormFile from, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var admin = await adminRepository.GetAsync(a => a.Id == id);

    var updateImage = await _fileService.UploadImageAsync(from);
    var adminUpdatedDto = new AdminUpdateDto()
    {
      ImagePath = updateImage
    };

    var result = await UpdateAsync(id, adminUpdatedDto);
    return result;
  }

  public async Task<bool> UpdatePasswordAsync(int id, PasswordUpdateDto dto, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var admin = await adminRepository.GetAsync(a => a.Id == id);

    if (admin is null)
      throw new StatusCodeException(System.Net.HttpStatusCode.NotFound, "Admin is not found");

    // _unitOfWork.Admins.TrackingDeteched(admin);
    // TrackingDeteched --> _dbContext.Entry<T>(entity!).State = EntityState.Detached;

    var isPasswordVerified = PasswordHasher.Verify(dto.OldPassword, admin.Salt, admin.PasswordHash);

    if (isPasswordVerified)
    {
      if (dto.NewPassword == dto.VerifyPassword)
      {
        var hash = PasswordHasher.Hash(dto.NewPassword);
        admin.PasswordHash = hash.Hash;
        admin.Salt = hash.Salt;
        admin.Id = id;

        await adminRepository.UpdateAsync(admin);

        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
      }
      else throw new StatusCodeException(HttpStatusCode.BadRequest, "new password and verify" + " password must be match!");
    }
    else throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid Password");
  }

  //pls make a new method that will name as CreateAdminAsync
  public async Task<bool> CreateAdminAsync(AdminRegisterDto dto, CancellationToken cancellationToken = default)
  {
    var admin = (Admin)dto;
    var hash = PasswordHasher.Hash(dto.Password);
    admin.PasswordHash = hash.Hash;
    admin.Salt = hash.Salt;
    admin.CreatedAt = TimeHelper.GetCurrentServerTime();
    admin.UpdatedAt = TimeHelper.GetCurrentServerTime();
    admin.Status = StatusType.Active;

    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    await adminRepository.AddAsync(admin);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }
}

