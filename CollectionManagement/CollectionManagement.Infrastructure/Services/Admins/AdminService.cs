using CollectionManagement.Infrastructure.Interfaces.Admins;
using CollectionManagement.Infrastructure.Interfaces.Files;

namespace CollectionManagement.Infrastructure.Services.Admins;

public class AdminService(IUnitOfWorkAsync unitOfWork,
                          IIdentityService identityService,
                          IFileService fileService)
  : IAdminService
{
  private readonly IUnitOfWorkAsync _unitOfWork;
  private readonly IIdentityService _identityService;
  private readonly IFileService _fileService;

  public async Task<bool> DeleteAsync(List<int> ids, CancellationToken cancellationToken = default)
  {
    foreach (var id in ids)
    {
      var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
      var temp = await adminRepository.GetAsync(a => a.Id == id);

      if (temp is not null)
      {
        adminRepository.Remove(a => a.Id == id);
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
          adminRepository.Update(temp);
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
          adminRepository.Update(temp);
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
      adminRepository.Update(admin);
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

  public Task<PagedResults<AdminViewModel>> GetPagedAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> UpdateAsync(int id, AdminUpdateDto adminUpdateDto, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> UpdateImageAsync(int id, IFormFile from, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> UpdatePasswordAsync(int id, PasswordUpdateDto dto, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> CreateAdminAsync(AdminRegisterDto adminCreateDto, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  /* Rewrite all of these
  public async Task<PagedList<AdminViewModel>> GetAllAsync(PaginationParams @params)
  {
    var query = _unitOfWork.Admins.GetAll().OrderBy(x => x.Id)
        .Select(x => _mapper.Map<AdminViewModel>(x));
    return await PagedList<AdminViewModel>.ToPagedListAsync(query, @params);
  }

  public async Task<bool> UpdateAsync(int id, AdminUpdateDto adminUpdateDto)
  {
    var admin = await _unitOfWork.Admins.FindByIdAsync(id);
    if (admin is null) throw new NotFoundException("Admin", $"{id} not found");
    _unitOfWork.Admins.TrackingDeteched(admin);
    if (adminUpdateDto != null)
    {
      admin.UserName = String.IsNullOrEmpty(adminUpdateDto.UserName) ? admin.UserName : adminUpdateDto.UserName;
      admin.Image = String.IsNullOrEmpty(adminUpdateDto.ImagePath) ? admin.Image : adminUpdateDto.ImagePath;
      admin.BirthDate = admin.BirthDate;
      admin.Address = String.IsNullOrEmpty(adminUpdateDto.Address) ? admin.Address : adminUpdateDto.Address;
      if (adminUpdateDto.Image is not null)
      {
        admin.Image = await _fileService.UploadImageAsync(adminUpdateDto.Image);
      }
      admin.LastUpdatedAt = TimeHelper.GetCurrentServerTime();
      _unitOfWork.Admins.Update(id, admin);
      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else throw new ModelErrorException("", "Not found");
  }

  public async Task<bool> UpdateImageAsync(int id, IFormFile from, CancellationToken cancellationToken = default)
  {
    var admin = await _unitOfWork.Admins.FindByIdAsync(id);
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
    var admin = await _unitOfWork.Admins.FindByIdAsync(id);
    if (admin is null)
      throw new StatusCodeException(System.Net.HttpStatusCode.NotFound, "Admin is not found");
    _unitOfWork.Admins.TrackingDeteched(admin);
    var res = PasswordHasher.Verify(dto.OldPassword, admin.Salt, admin.PasswordHash);
    if (res)
    {
      if (dto.NewPassword == dto.VerifyPassword)
      {
        var hash = PasswordHasher.Hash(dto.NewPassword);
        admin.PasswordHash = hash.Hash;
        admin.Salt = hash.Salt;
        _unitOfWork.Admins.Update(id, admin);
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
      }
      else throw new StatusCodeException(System.Net.HttpStatusCode.BadRequest, "new password and verify" + " password must be match!");
    }
    else throw new StatusCodeException(System.Net.HttpStatusCode.BadRequest, "Invalid Password");
  }

  //pls make a new method that will name as CreateAdminAsync
  public async Task<bool> CreateAdminAsync(AdminRegisterDto dto, CancellationToken cancellationToken = default)
  {
    var admin = _mapper.Map<Admin>(dto);
    var hash = PasswordHasher.Hash(dto.Password);
    admin.PasswordHash = hash.Hash;
    admin.Salt = hash.Salt;
    admin.CreatedAt = TimeHelper.GetCurrentServerTime();
    admin.LastUpdatedAt = TimeHelper.GetCurrentServerTime();
    admin.Status = StatusType.Active;
    _unitOfWork.Admins.Add(admin);
    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }
  */
}

