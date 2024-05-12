namespace CollectionManagement.Infrastructure.Services.Users;

public class UserService(IUnitOfWorkAsync unitOfWork,
                         IAuthService authService,
                         IImageService imageService,
                         IIdentityService identityService)
  : IUserService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
  private readonly IAuthService _authService = authService;
  private readonly IImageService _imageService = imageService;
  private readonly IIdentityService _identityService = identityService;

  public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(u => u.Id == id);

    if (user is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");

    await userRepository.RemoveAsync(u => u.Id == id);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> DeleteImageAsync(int id, CancellationToken cancellationToken = default)
  {
    //var user = await _unitOfWork.Users.FindByIdAsync(id);
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(u => u.Id == id);

    await _imageService.DeleteImageAsync(user!.Image);
    user.Image = "";

    userRepository.UpdateAsync(user);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> ImageUpdateAsync(int id, IFormFile file, CancellationToken cancellationToken = default)
  {
    //var user = await _unitOfWork.Users.FindByIdAsync(id);
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(u => u.Id == id);

    if (user == null)
      throw new StatusCodeException(System.Net.HttpStatusCode.NotFound, "user is not found");

    // _unitOfWork.Users.TrackingDeteched(user);
    // TrackingDeteched --> _dbContext.Entry<T>(entity!).State = EntityState.Detached;

    if (user.Image != null)
    {
      await _imageService.DeleteImageAsync(user.Image);
    }

    user.Image = await _imageService.SaveImageAsync(file);

    userRepository.UpdateAsync(user);

    int result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> UpdateAsync(int id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var user = await userRepository.GetAsync(u => u.Id == id);

    if (user is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Student is not found");
    else
    {
      // _unitOfWork.Users.TrackingDeteched(user);
      // TrackingDeteched --> _dbContext.Entry<T>(entity!).State = EntityState.Detached;

      if (userUpdateDto != null)
      {
        user.UserName = String.IsNullOrEmpty(userUpdateDto.UserName) ? user.UserName : userUpdateDto.UserName;
        user.BirthDate = userUpdateDto.BirthDate;
        user.Image = String.IsNullOrEmpty(userUpdateDto.ImagePath) ? user.Image : userUpdateDto.ImagePath;
        if (userUpdateDto.Image != null)
        {
          user.Image = await _imageService.SaveImageAsync(userUpdateDto.Image);
        }
      }
      user.UpdatedAt = TimeHelper.GetCurrentServerTime();
      userRepository.UpdateAsync(user);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
  }

  public async Task<bool> UpdatePasswordAsync(int id, PasswordUpdateDto dto, CancellationToken cancellationToken = default)
  {
    var adminRepository = await _unitOfWork.GetRepositoryAsync<Admin>();
    var admin = await adminRepository.GetAsync(a => a.Id == id);

    if (admin is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Admin is not found");

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

        adminRepository.UpdateAsync(admin);

        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
      }
      else throw new StatusCodeException(System.Net.HttpStatusCode.BadRequest, "new password and verify" + " password must be match!");
    }
    else throw new StatusCodeException(System.Net.HttpStatusCode.BadRequest, "Invalid Password");
  }
  public async Task<PagedResults<CollectionViewModel>> GetPagedCollectionsAsync(PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var userId = _identityService.Id ?? 0;

    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var likeRepository = await _unitOfWork.GetRepositoryAsync<Like>();

    var collections = await collectionRepository.GetAllAsync(predicates: [c => c.UserId == userId],
                                                             orderBy: c => c.Id);
    var likes = await likeRepository.GetAllAsync();

    var query = likes.Join(collections, like => like.CollectionId, collection => collection.Id, (like, collection) =>
    {
      var isLiked = likes.Any(l => l.CollectionId == collection.Id);
      var likeCount = likes.Where(l => l.CollectionId == collection.Id).Count();

      return new CollectionViewModel()
      {
        Id = collection.Id,
        Name = collection.Name,
        Description = collection.Description,
        ImagePath = collection.Image,
        LikeCount = likeCount,
        UserId = userId,
        IsLiked = isLiked
      };
    });

    return query.OrderByDescending(q => q.LikeCount)
                .ToPagedResult(@params.PageSize, @params.PageNumber);
  }

  public async Task<PagedResults<ItemViewModel>> GetPagedItemsAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default)
  {

    var userId = _identityService.Id ?? 0;

    var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
    var likeItemRepository = await _unitOfWork.GetRepositoryAsync<LikeItem>();

    var items = await itemRepository.GetAllAsync(predicates: [i => i.UserId == userId && i.CollectionId == id],
                                                             orderBy: i => i.Id);
    var likeItems = await likeItemRepository.GetAllAsync();

    var query = likeItems.Join(items, likeItem => likeItem.ItemId, item => item.Id, (likeItem, item) =>
    {
      var isLiked = likeItems.Any(l => l.UserId == userId && l.ItemId == item.Id);
      var likeCount = likeItems.Where(l => l.ItemId == item.Id).Count();

      return new ItemViewModel()
      {
        Id = item.Id,
        Name = item.Name,
        Description = item.Description,
        ImagePath = item.Image,
        LikeCount = likeCount,
        CollectionId = item.CollectionId,
        UserId = userId,
        IsLiked = isLiked != null
      };
    });

    return query.OrderByDescending(q => q.LikeCount)
                .ToPagedResult(@params.PageSize, @params.PageNumber);
  }

  public async Task<PagedResults<UserViewModel>> GetPagedUsersAsync(PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var userRepository = await _unitOfWork.GetRepositoryAsync<User>();
    var users = await userRepository.GetAllAsync(orderBy: u => u.Id);
    var query = users.Select(u => (UserViewModel)u);

    return query.ToPagedResult(@params.PageSize, @params.PageNumber);
  }
  public async Task<CollectionViewModel> GetCollectionByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    //var userid = _identityService.Id ?? 0;
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == id);

    if (collection is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Collection not found");

    /*if (res.UserId != userid)
        throw new StatusCodeException(HttpStatusCode.Forbidden, "You are not authorized to access this collection");*/

    var result = (CollectionViewModel)collection;
    return result;
  }

  public async Task<ItemViewModel> GetItemByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
    var item = await itemRepository.GetAsync(i => i.Id == id);

    if (item is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Item not found");

    var result = (ItemViewModel)item;
    return result;
  }

  public async Task<PagedResults<CommentViewModel>> GetPagedCommentsAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default)

  {
    //var userid = _identityService.Id ?? 0;

    var commentRepository = await _unitOfWork.GetRepositoryAsync<Comment>();
    var comments = await commentRepository.GetAllAsync(predicates: [c => c.ItemId == id],
                                                       orderBy: c => c.Id);
    var query = comments.Select(c => (CommentViewModel)c);

    //return await PagedList<CommentViewModel>.ToPagedListAsync(query, @params);
    return query.ToPagedResult(@params.PageSize, @params.PageNumber);
  }
}

