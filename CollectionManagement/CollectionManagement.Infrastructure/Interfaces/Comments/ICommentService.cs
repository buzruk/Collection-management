namespace CollectionManagement.Infrastructure.Interfaces.Comments;

public interface ICommentService
{
  Task<bool> CreateCommentAsync(CommentDto commentDto, CancellationToken cancellationToken = default);

  Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);
}

