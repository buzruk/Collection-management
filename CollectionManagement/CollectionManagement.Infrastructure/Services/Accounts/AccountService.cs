namespace CollectionManagement.Infrastructure.Services.Accounts;

public class AccountService(IUnitOfWorkAsync unitOfWork,
                            IAuthService authService,
                            IMemoryCache memoryCache)
  : IAccountService
{
  private readonly IMemoryCache _memoryCache = memoryCache;
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
  private readonly IAuthService _authService = authService;

  public async Task<bool> AdminRegisterAsync(AdminRegisterDto adminRegisterDto, 
                                             CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var emailcheck = await adminRepository.GetAsync(a => a.Email == adminRegisterDto.Email,
                                                    cancellationToken: cancellationToken);

    if (emailcheck is not null)
      throw new StatusCodeException(HttpStatusCode.Conflict, "Email alredy exist");

    var hashresult = PasswordHasher.Hash(adminRegisterDto.Password);
    var admin = (Admin)adminRegisterDto;
    admin.AdminRole = RoleConstants.Admin;
    admin.PasswordHash = hashresult.Hash;
    admin.Salt = hashresult.Salt;
    admin.CreatedAt = TimeHelper.GetCurrentServerTime();
    admin.UpdatedAt = TimeHelper.GetCurrentServerTime();
    await adminRepository.AddAsync(admin, cancellationToken);
    var result = await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
    return result > 0;
  }
  public async Task<bool> RegisterAsync(AccountRegisterDto registerDto, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var emailcheck = await userRepository.GetAsync(a => a.Email == registerDto.Email, 
                                                   cancellationToken: cancellationToken);

    if (emailcheck is not null)
      throw new StatusCodeException(HttpStatusCode.Conflict, "Email alredy exist");

    var hasherResult = PasswordHasher.Hash(registerDto.Password);
    var user = (User)registerDto;
    user.UserRole = RoleConstants.User;
    user.PasswordHash = hasherResult.Hash;
    user.Salt = hasherResult.Salt;
    user.Status = StatusType.Active;
    user.CreatedAt = TimeHelper.GetCurrentServerTime();
    user.UpdatedAt = TimeHelper.GetCurrentServerTime();
    await userRepository.AddAsync(user, cancellationToken);
    var result = await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
    return result > 0;
  }
  public async Task<string> LoginAsync(AccountLoginDto accountLoginDto, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var admin = await adminRepository.GetAsync(a => a.Email == accountLoginDto.Email, 
                                               cancellationToken: cancellationToken);

    if (admin is null)
    {
      var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
      var user = await userRepository.GetAsync(a => a.Email == accountLoginDto.Email, 
                                               cancellationToken: cancellationToken);

      //var user = await _repository.Users.FirstOrDefault(x => x.Email == accountLoginDto.Email);
      if (user is null) throw new NotFoundException(nameof(accountLoginDto.Email), "No user with this email is found!");
      if (user.Status != StatusType.Blocked)
      {
        var hasherResult = PasswordHasher.Verify(accountLoginDto.Password, user.Salt, user.PasswordHash);
        if (hasherResult)
        {
          string token = _authService.GenerateToken(user, "user");
          return token;
        }
        else throw new NotFoundException(nameof(accountLoginDto.Password), "Incorrect password!");
      }
      else throw new NotFoundException(nameof(accountLoginDto.Email), "User is blocked!");
    }
    else
    {
      var hasherResult = PasswordHasher.Verify(accountLoginDto.Password, admin.Salt, admin.PasswordHash);
      if (hasherResult)
      {
        string token = "";
        if (admin.Email != null)
        {
          token = _authService.GenerateToken(admin, "admin");
          return token;
        }
        token = _authService.GenerateToken(admin, "admin");
        return token;
      }
      else throw new NotFoundException(nameof(accountLoginDto.Password), "Incorrect password!");
    }
  }
  public async Task<bool> DeleteByPasswordAsync(UserDeleteDto userDeleteDto, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(a => a.Id == HttpContextHelper.UserId, 
                                             cancellationToken: cancellationToken);

    if (user is null) throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");

    var result = PasswordHasher.Verify(userDeleteDto.Password, user.Salt, user.PasswordHash);
    if (!result) throw new StatusCodeException(HttpStatusCode.NotFound, "Password is incorrect!");

    return true;
  }
  public async Task<bool> PasswordUpdateAsync(PasswordUpdateDto passwordUpdateDto, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(a => a.Id == HttpContextHelper.UserId, 
                                             cancellationToken: cancellationToken);

    if (user is not null)
    {
      var isPasswordVerified = PasswordHasher.Verify(passwordUpdateDto.OldPassword, user.Salt, user.PasswordHash);
      if (isPasswordVerified)
      {
        if (passwordUpdateDto.NewPassword == passwordUpdateDto.VerifyPassword)
        {
          var hash = PasswordHasher.Hash(passwordUpdateDto.VerifyPassword);
          user.Salt = hash.Salt;
          user.PasswordHash = hash.Hash;
          await Task.Run(() => userRepository.Update(user), cancellationToken);
          var result = await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
          return result > 0;
        }
        else throw new StatusCodeException(HttpStatusCode.BadRequest, "new password and verify password must be match");
      }
      else throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid Password");
    }
    else throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");
  }
}

