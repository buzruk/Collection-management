namespace CollectionManagement.Infrastructure.Services.Comments;

public class CommentService(IUnitOfWorkAsync unitOfWork, IIdentityService identityService) 
  : ICommentService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;
  private readonly IIdentityService _identityService = identityService;

  public async Task<bool> CreateCommentAsync(CommentDto commentDto, CancellationToken cancellationToken = default)
  {
    var userid = _identityService.Id ?? 0;
    var entity = new Comment
    {
      Content = commentDto.CommentText,
      UserId = userid,
      ItemId = commentDto.ItemId,
      CreatedAt = TimeHelper.GetCurrentServerTime()
    };

    var commentRepository = await _unitOfWork.GetRepositoryAsync<Comment>();
    await commentRepository.AddAsync(entity);

    var result = await _unitOfWork.SaveChangesAsync();
    return result > 0;
  }

  public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
    {
      throw new ArgumentException("Invalid tweet ID");
    }

    //var tweetToDelete = await _unitOfWork.Comments.FindByIdAsync(id);
    var commentRepository = await _unitOfWork.GetRepositoryAsync<Comment>();
    var comment = await commentRepository.GetAsync(c => c.Id == id);


    if (comment is null)
    {
      throw new StatusCodeException(HttpStatusCode.NotFound, "Tweet not found");
    }

    var userId = _identityService.Id ?? 0;

    if (userId == comment?.UserId)
    {
      await commentRepository.RemoveAsync(c => c.Id == id);

      var result = await _unitOfWork.SaveChangesAsync();
      return result > 0;
    }
    else
    {
      throw new StatusCodeException(HttpStatusCode.BadRequest, "You are not authorized to delete this comment");
    }
  }
}

