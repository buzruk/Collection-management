using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services;

public class ItemService(IUnitOfWorkAsync unitOfWork,
                         IIdentityService identityService,
                         IWebHostEnvironment webHostEnvironment,
                         IConfiguration configuration,
                         IImageService imageService)
  : IItemService
{
    private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
    private readonly IImageService _imageService = imageService;
    private readonly IIdentityService _identityService = identityService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> AddAsync(ItemDto itemDto, CancellationToken cancellationToken = default)
    {
        var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        var domain = _configuration["Domain"] ?? "";

        var userId = _identityService.Id;

        var collectionRepository = await _unitOfWork.GetRepositoryAsync<Collection>();
        var userInCollection = await collectionRepository.GetAsync(c => c.Id == itemDto.CollectionId);

        if (userId == userInCollection?.UserId)
        {
            var imagepath = await _imageService.UploadAsync(itemDto.Image!, folder, domain);
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
    public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        var userId = _identityService.Id;
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

    public async Task<ItemViewModel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
        var item = await itemRepository.GetAsync(i => i.Id == id);

        if (item is null)
            throw new StatusCodeException(HttpStatusCode.NotFound, "Item not found");

        var result = (ItemViewModel)item;
        return result;
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
                IsLiked = isLiked
            };
        });

        return query.OrderByDescending(q => q.LikeCount)
                    .ToPagedResult(@params.PageSize, @params.PageNumber);
    }

    //public async Task<PagedResults<ItemViewModel>> GetPagedAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default)
    //{

    //  var userId = _identityService.Id ?? 0;

    //  var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
    //  var likeItemRepository = await _unitOfWork.GetRepositoryAsync<LikeItem>();

    //  var items = await itemRepository.GetAllAsync(predicates: [i => i.UserId == userId && i.CollectionId == id],
    //                                                           orderBy: i => i.Id);
    //  var likeItems = await likeItemRepository.GetAllAsync();

    //  var query = likeItems.Join(items, likeItem => likeItem.ItemId, item => item.Id, (likeItem, item) =>
    //  {
    //    var isLiked = likeItems.Any(l => l.UserId == userId && l.ItemId == item.Id);
    //    var likeCount = likeItems.Where(l => l.ItemId == item.Id).Count();

    //    return new ItemViewModel()
    //    {
    //      Id = item.Id,
    //      Name = item.Name,
    //      Description = item.Description,
    //      ImagePath = item.Image,
    //      LikeCount = likeCount,
    //      CollectionId = item.CollectionId,
    //      UserId = userId,
    //      IsLiked = isLiked
    //    };
    //  });

    //  return query.OrderByDescending(q => q.LikeCount)
    //              .ToPagedResult(@params.PageSize, @params.PageNumber);
    //}

    public async Task<bool> UpdateAsync(int id,
                                        ItemUpdateDto item,
                                        CancellationToken cancellationToken = default)
    {

        var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        var domain = _configuration["Domain"] ?? "";

        var userId = _identityService.Id;
        var itemRepository = await _unitOfWork.GetRepositoryAsync<Item>();
        var userInItem = await itemRepository.GetAsync(i => i.Id == id);

        if (userId == userInItem?.UserId)
        {
            if (userInItem is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Item not found");

            userInItem.Name = item.Name;
            userInItem.Description = item.Description;
            userInItem.Image = string.IsNullOrEmpty(item.ImagePath) ? userInItem.Image : item.ImagePath;

            if (item.Image is not null)
            {
                userInItem.Image = await _imageService.UploadAsync(item.Image, folder, domain);
            }
            userInItem.UpdatedAt = TimeHelper.GetCurrentServerTime();

            await itemRepository.UpdateAsync(userInItem);

            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        else
            throw new StatusCodeException(HttpStatusCode.BadRequest, "You are not authorized to update this item");
    }
    public async Task<bool> UpdateImageAsync(int id,
                                             IFormFile formFile,
                                             CancellationToken cancellationToken = default)
    {
        var folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        var domain = _configuration["Domain"] ?? "";

        var updateImage = await _imageService.UploadAsync(formFile, folder, domain);
        var collectionUpdateDto = new ItemUpdateDto()
        {
            ImagePath = updateImage
        };
        var result = await UpdateAsync(id, collectionUpdateDto);
        return result;
    }
}

