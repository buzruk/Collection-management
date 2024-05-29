namespace CollectionManagement.Infrastructure.Repositories;

public class CollectionRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Collection>(dbContext),
    ICollectionRepositoryAsync
{
}
