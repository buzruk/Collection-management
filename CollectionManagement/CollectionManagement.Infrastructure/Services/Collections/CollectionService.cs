namespace CollectionManagement.Infrastructure.Services.Collections;

public class CollectionService(IUnitOfWorkAsync unitOfWork,
                               IIdentityService identityService,
                               IFileService fileService,
                               IImageService imageService)
: ICollectionService
{
  private readonly IImageService _imageService = imageService;
  private readonly IFileService _fileService = fileService;
  private readonly IIdentityService _identityService = identityService;
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;

  public async Task<PagedResults<CollectionViewModel>> GetPagedAsync(PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
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

  public async Task<bool> CreateCollectionAsync(CollectionDto collectionCreateDto, CancellationToken cancellationToken = default)
  {
    var userid = _identityService.Id ?? 0;
    var imagepath = await _imageService.SaveImageAsync(collectionCreateDto.Image!);
    var entity = new Collection
    {
      Name = collectionCreateDto.Name,
      Description = collectionCreateDto.Description,
      Topic = collectionCreateDto.Topics,
      Image = imagepath,
      UserId = userid,
      CreatedAt = TimeHelper.GetCurrentServerTime(),
      UpdatedAt = TimeHelper.GetCurrentServerTime()
    };
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    await collectionRepository.AddAsync(entity);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> DeleteCollectionAsync(int id, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var collection = collectionRepository.GetAsync(c => c.Id == id);

    if (collection is null)
      throw new StatusCodeException(HttpStatusCode.NotFound, "Collection not found");

    await collectionRepository.RemoveAsync(c => c.Id == id);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> UpdateCollectionAsync(int id, CollectionUpdateDto collectionUpdateDto, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == id);

    if (collection is null)
      throw new StatusCodeException(System.Net.HttpStatusCode.NotFound, "Collection not found");

    collection.Name = collectionUpdateDto.Name;
    collection.Description = collectionUpdateDto.Description;
    collection.Topic = collectionUpdateDto.Topics;
    collection.Image = String.IsNullOrEmpty(collectionUpdateDto.ImagePath) ? collection.Image : collectionUpdateDto.ImagePath;

    if (collectionUpdateDto.Image is not null)
    {
      collection.Image = await _fileService.UploadImageAsync(collectionUpdateDto.Image);
    }
    collection.UpdatedAt = TimeHelper.GetCurrentServerTime();
    collection.Id = id;
    await collectionRepository.UpdateAsync(collection);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> UpdateImageAsync(int id, IFormFile formFile, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == id);

    var updateImage = await _fileService.UploadImageAsync(formFile);

    var collectionUpdateDto = new CollectionUpdateDto()
    {
      ImagePath = updateImage
    };

    var result = await UpdateCollectionAsync(id, collectionUpdateDto);
    return result;
  }

  public async Task<PagedResults<CollectionViewModel>> SearchAsync(PaginationParams @params, string name, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var likeRepository = await _unitOfWork.GetRepositoryAsync<Like>();

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

  public async Task<PagedResults<CollectionViewModel>> TopCollection(PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var likeRepository = await _unitOfWork.GetRepositoryAsync<Like>();

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

  public async Task<bool> GetCollectionById(int userId, int collectionId, CancellationToken cancellationToken = default)
  {
    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var collection = await collectionRepository.GetAsync(c => c.Id == collectionId);


    if (collection is null)
      throw new StatusCodeException(System.Net.HttpStatusCode.NotFound, "Collection not found");

    return collection.UserId == userId;
  }
}

