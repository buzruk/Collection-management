namespace CollectionManagement.Infrastructure.Repositories;

public class LikeItemRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, LikeItem>(dbContext),
    ILikeItemRepositoryAsync
{
}
