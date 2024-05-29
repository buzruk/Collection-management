namespace CollectionManagement.Infrastructure.Repositories;

public class LikeRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Like>(dbContext),
    ILikeRepositoryAsync
{
}
