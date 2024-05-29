using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services;

public class CollectionService(IUnitOfWorkAsync unitOfWork,
                               IHttpContextAccessor httpContextAccessor,
                               UserManager<User> userManager,
                               IConfiguration configuration,
                               IWebHostEnvironment webHostEnvironment,
                               IIdentityService identityService,
                               IImageService imageService)
: ICollectionService
{
  private readonly IImageService _imageService = imageService;
  private readonly IIdentityService _identityService = identityService;
  private readonly IUnitOfWorkAsync _unitOfWorkAsync = unitOfWork;
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  private readonly UserManager<User> _userManager = userManager;
  private readonly IConfiguration _configuration = configuration;
  private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

  public async Task<PagedResults<CollectionViewModel>> GetPagedAsync(PaginationParams @params,
                                                                     CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var pagedResult = await collectionRepository.GetPagedAsync(orderBy: a => a.Id,
                                                               pageNumber: @params.PageNumber,
                                                               pageSize: @params.PageSize);
    var result = new PagedResults<CollectionViewModel>()
    {
      Items = pagedResult.Items.Select(a => (CollectionViewModel)a),
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

  //public async Task<PagedResults<CollectionViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default)
  //{
  //  var userId = _identityService.Id ?? 0;

  //  var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
  //  var likeRepository = await _unitOfWorkAsync.GetRepositoryAsync<Like>();

  //  var collections = await collectionRepository.GetAllAsync(predicates: [c => c.UserId == userId],
  //                                                           orderBy: c => c.Id);
  //  var likes = await likeRepository.GetAllAsync();

  //  var query = likes.Join(collections, like => like.CollectionId, collection => collection.Id, (like, collection) =>
  //  {
  //    var isLiked = likes.Any(l => l.CollectionId == collection.Id);
  //    var likeCount = likes.Where(l => l.CollectionId == collection.Id).Count();

  //    return new CollectionViewModel()
  //    {
  //      Id = collection.Id,
  //      Name = collection.Name,
  //      Description = collection.Description,
  //      ImagePath = collection.Image,
  //      LikeCount = likeCount,
  //      UserId = userId,
  //      IsLiked = isLiked
  //    };
  //  });

  //  return query.OrderByDescending(q => q.LikeCount)
  //              .ToPagedResult(@params.PageSize, @params.PageNumber);
  //}

  public async Task<bool> AddAsync(CollectionDto dto,
                                   CancellationToken cancellationToken = default)
  {
    if (dto == null) throw new ArgumentNullException(nameof(dto));

    var userId = _identityService.Id;
    var user = await _userManager.FindByIdAsync(userId);

    if (user is null)
    {
      throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");
    }

    var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
    var domain = _configuration["Domain"] ?? "";
    string imagepath = "";

    if (dto.Image is not null)
    {
       imagepath = await _imageService.UploadAsync(dto.Image, folder, domain);
    }

    var entity = new Collection
    {
      Name = dto.Name,
      Description = dto.Description,
      Topic = dto.Topics,
      Image = imagepath,
      UserId = user.Id,
      CreatedAt = TimeHelper.GetCurrentServerTime(),
      UpdatedAt = TimeHelper.GetCurrentServerTime()
    };
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    await collectionRepository.AddAsync(entity);

    var result = await _unitOfWorkAsync.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var collection = collectionRepository.GetAsync(c => c.Id == id);

    if (collection is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Collection not found");

    await collectionRepository.RemoveAsync(c => c.Id == id);

    var result = await _unitOfWorkAsync.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> UpdateAsync(int id,
                                      CollectionUpdateDto dto,
                                      CancellationToken cancellationToken = default)
  {
    var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
    var domain = _configuration["Domain"] ?? "";

    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == id);

    if (collection is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Collection not found");

    collection.Name = dto.Name;
    collection.Description = dto.Description;
    collection.Topic = dto.Topics;
    collection.Image = string.IsNullOrEmpty(dto.ImagePath) ? collection.Image : dto.ImagePath;

    if (dto.Image is not null)
    {
      collection.Image = await _imageService.UploadAsync(dto.Image, folder, domain);
    }
    collection.UpdatedAt = TimeHelper.GetCurrentServerTime();
    collection.Id = id;
    await collectionRepository.UpdateAsync(collection);

    var result = await _unitOfWorkAsync.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> UpdateImageAsync(int id,
                                           IFormFile formFile,
                                           CancellationToken cancellationToken = default)
  {
    var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
    var domain = _configuration["Domain"] ?? "";
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == id);

    var updateImage = await _imageService.UploadAsync(formFile, folder, domain);

    var collectionUpdateDto = new CollectionUpdateDto()
    {
      ImagePath = updateImage
    };

    var result = await UpdateAsync(id, collectionUpdateDto);
    return result;
  }

  public async Task<PagedResults<CollectionViewModel>> SearchAsync(PaginationParams @params,
                                                                   string name,
                                                                   CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var likeRepository = await _unitOfWorkAsync.GetRepositoryAsync<Like>();

    var collections = await collectionRepository.GetAllAsync([ c =>
        c.Name.ToLower().StartsWith(name.ToLower())
        ||
        c.Description.ToLower().StartsWith(name.ToLower())
      ]);
    var likes = await likeRepository.GetAllAsync();

    var query = likes.Join(collections, like => like.CollectionId, collection => collection.Id, (like, collection) =>
    {
      var likeCount = likes.Where(l => l.CollectionId == collection.Id).Count();

      return new CollectionViewModel()
      {
        Id = collection.Id,
        Name = collection.Name,
        Description = collection.Description,
        Topics = collection.Topic,
        ImagePath = collection.Image,
        UserId = collection.UserId,
        CreatedAt = collection.CreatedAt,
        LastUpdatedAt = collection.UpdatedAt,
        LikeCount = likeCount
      };
    });

    return query.OrderByDescending(q => q.LikeCount)
                .ToPagedResult(@params.PageSize, @params.PageNumber);
  }

  public async Task<PagedResults<CollectionViewModel>> TopCollection(PaginationParams @params,
                                                                     CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var likeRepository = await _unitOfWorkAsync.GetRepositoryAsync<Like>();

    var collections = await collectionRepository.GetAllAsync();
    var likes = await likeRepository.GetAllAsync();

    var query = likes.Join(collections, like => like.CollectionId, collection => collection.Id, (like, collection) =>
    {
      var likeCount = likes.Where(l => l.CollectionId == collection.Id).OrderDescending().Count();

      return new CollectionViewModel()
      {
        Id = collection.Id,
        Name = collection.Name,
        Description = collection.Description,
        Topics = collection.Topic,
        ImagePath = collection.Image,
        UserId = collection.UserId,
        CreatedAt = collection.CreatedAt,
        LastUpdatedAt = collection.UpdatedAt,
        LikeCount = likeCount
      };
    });

    return query.ToPagedResult(@params.PageSize, @params.PageNumber);
  }

  public async Task<CollectionViewModel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    //var userid = _identityService.Id ?? 0;
    var collectionRepository = await _unitOfWorkAsync.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == id);

    if (collection is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Collection not found");

    /*if (res.UserId != userid)
        throw new StatusCodeException(HttpStatusCode.Forbidden, "You are not authorized to access this collection");*/

    var result = (CollectionViewModel)collection;
    return result;
  }
}

