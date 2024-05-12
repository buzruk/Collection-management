namespace CollectionManagement.Infrastructure.Services.Items;

public class ItemService(IUnitOfWorkAsync unitOfWork,
                         IIdentityService identityService,
                         IImageService imageService,
                         IFileService fileService)
  : IItemService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
  private readonly IFileService _fileService = fileService;
  private readonly IImageService _imageService = imageService;
  private readonly IIdentityService _identityService = identityService;

  public async Task<bool> CreateItemAsync(ItemDto itemDto, CancellationToken cancellationToken = default)
  {
    var userId = _identityService.Id ?? 0;

    var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
    var userInCollection = await collectionRepository.GetAsync(c => c.Id == itemDto.CollectionId);

    if (userId == userInCollection?.UserId)
    {
      var imagepath = await _imageService.SaveImageAsync(itemDto.Image!);
      var entity = new Item
      {
        Name = itemDto.Name,
        Description = itemDto.Description,
        UserId = userId,
        Image = imagepath,
        CollectionId = itemDto.CollectionId
      };

      var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
      await itemRepository.AddAsync(entity);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else
      throw new StatusCodeException(HttpStatusCode.BadRequest, "You are not authorized to create an item in this collection");
  }
  public async Task<bool> DeleteItemAsync(int id, CancellationToken cancellationToken = default)
  {
    var userId = _identityService.Id ?? 0;
    var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
    var userInItem = await itemRepository.GetAsync(i => i.Id == id);

    if (userId == userInItem?.UserId)
    {
      var item = await itemRepository.GetAsync(i => i.Id == id);

      if (item is null)
        throw new StatusCodeException(HttpStatusCode.NotFound, "Item not found");

      await itemRepository.RemoveAsync(i => i.Id == id);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else
      throw new StatusCodeException(HttpStatusCode.BadRequest, "You are not authorized to delete this item");

  }
  public async Task<PagedResults<ItemViewModel>> GetPagedAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default)
  {
    var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
    var likeItemRepository = await _unitOfWork.GetRepositoryAsync<LikeItem>();

    var items = await itemRepository.GetAllAsync(predicates: [i => i.CollectionId == id],
                                                 orderBy: i => i.Id);
    var likeItems = await likeItemRepository.GetAllAsync();

    var query = likeItems.Join(items, like => like.ItemId, item => item.Id, (likeItem, item) =>
    {
      var isLiked = likeItems.Any(l => l.ItemId == item.Id);
      var likeCount = likeItems.Where(l => l.ItemId == item.Id).Count();

      return new ItemViewModel()
      {
        Id = item.Id,
        Name = item.Name,
        Description = item.Description,
        ImagePath = item.Image,
        LikeCount = likeCount,
        CollectionId = item.CollectionId,
        IsLiked = isLiked != null
      };
    });

    return query.OrderByDescending(q => q.LikeCount)
                .ToPagedResult(@params.PageSize, @params.PageNumber);
  }
  public async Task<bool> UpdateItemAsync(int id, ItemUpdateDto item, CancellationToken cancellationToken = default)
  {
    var userId = _identityService.Id ?? 0;
    var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
    var userInItem = await itemRepository.GetAsync(i => i.Id == id);

    if (userId == userInItem?.UserId)
    {
      if (userInItem is null)
        throw new StatusCodeException(HttpStatusCode.NotFound, "Item not found");

      userInItem.Name = item.Name;
      userInItem.Description = item.Description;
      userInItem.Image = String.IsNullOrEmpty(item.ImagePath) ? userInItem.Image : item.ImagePath;
      //userInItem.Id = id;

      if (item.Image is not null)
      {
        userInItem.Image = await _fileService.UploadImageAsync(item.Image);
      }
      userInItem.UpdatedAt = TimeHelper.GetCurrentServerTime();

      await itemRepository.UpdateAsync(userInItem);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else
      throw new StatusCodeException(HttpStatusCode.BadRequest, "You are not authorized to update this item");
  }
  public async Task<bool> UpdateImageAsync(int id, IFormFile formFile, CancellationToken cancellationToken = default)
  {
    var updateImage = await _fileService.UploadImageAsync(formFile);
    var collectionUpdateDto = new ItemUpdateDto()
    {
      ImagePath = updateImage
    };
    var result = await UpdateItemAsync(id, collectionUpdateDto);
    return result;
  }
}

