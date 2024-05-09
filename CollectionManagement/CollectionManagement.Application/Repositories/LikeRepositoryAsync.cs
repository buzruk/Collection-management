namespace CollectionManagement.Application.Repositories;

public class LikeRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Like>(dbContext),
    ILikeRepositoryAsync
{
}
