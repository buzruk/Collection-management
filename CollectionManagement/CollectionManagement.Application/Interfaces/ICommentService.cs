namespace CollectionManagement.Application.Interfaces;

public interface ICommentService
{
    Task<bool> AddAsync(CommentDto dto, CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedResults<CommentViewModel>> GetPagedAsync(int id, PaginationParams @params, CancellationToken cancellationToken = default);
}

