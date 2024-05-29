namespace CollectionManagement.Infrastructure.Repositories;

public class CommentRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Comment>(dbContext),
    ICommentRepositoryAsync
{
}
