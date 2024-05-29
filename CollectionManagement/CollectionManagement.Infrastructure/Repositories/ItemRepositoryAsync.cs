namespace CollectionManagement.Infrastructure.Repositories;

public class ItemRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Item>(dbContext),
    IItemRepositoryAsync
{
}
