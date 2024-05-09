namespace CollectionManagement.Application.Repositories;

public class CommentRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Comment>(dbContext),
    ICommentRepositoryAsync
{
}
