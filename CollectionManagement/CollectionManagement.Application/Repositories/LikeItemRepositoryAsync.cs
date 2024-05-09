namespace CollectionManagement.Application.Repositories;

public class LikeItemRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, LikeItem>(dbContext),
    ILikeItemRepositoryAsync
{
}
