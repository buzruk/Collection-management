namespace CollectionManagement.Application.Repositories;

public class CollectionRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Collection>(dbContext),
    ICollectionRepositoryAsync
{
}
