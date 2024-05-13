using CollectionManagement.Infrastructure.Interfaces.Likes;

namespace CollectionManagement.Infrastructure.Services.Likes;

public class LikeService(IUnitOfWorkAsync unitOfWork, IIdentityService identityService) 
  : ILikeService
{
  private readonly IIdentityService _identityService = identityService;
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;

  public async Task<bool> ToggleCollection(int collectionId, CancellationToken cancellationToken = default)
  {
    var userid = _identityService.Id ?? 0;
    var likeRepository = await _unitOfWork.GetRepositoryAsync<Like>();
    var like = await likeRepository.GetAsync(l => l.CollectionId == collectionId && l.UserId == userid);

    if (like != null && userid == like?.UserId)
    {
      await likeRepository.RemoveAsync(l => l.Id == like.Id);
    }
    else
    {
      var entity = new Like
      {
        UserId = userid,
        CollectionId = collectionId,
        CreatedAt = TimeHelper.GetCurrentServerTime()
      };
      likeRepository.AddAsync(entity);
    }

    await _unitOfWork.SaveChangesAsync();

    return true;
  }
  public async Task<bool> ToggleItem(int itemId, CancellationToken cancellationToken = default)
  {
    var userid = _identityService.Id ?? 0;
    var likeItemRepository = await _unitOfWork.GetRepositoryAsync<LikeItem>();
    var likeItem = await likeItemRepository.GetAsync(l => l.ItemId == itemId && l.UserId == userid);

    if (likeItem != null && userid == likeItem?.UserId)
    {
      await likeItemRepository.RemoveAsync(l => l.Id == likeItem.Id);
    }
    else
    {
      var entity = new LikeItem
      {
        UserId = userid,
        ItemId = itemId,
        CreatedAt = TimeHelper.GetCurrentServerTime()
      };
      await likeItemRepository?.AddAsync(entity);
    }

    await _unitOfWork.SaveChangesAsync();
    return true;
  }
}

