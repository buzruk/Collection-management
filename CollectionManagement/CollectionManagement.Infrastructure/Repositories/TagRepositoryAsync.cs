namespace CollectionManagement.Infrastructure.Repositories;

public class TagRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Tag>(dbContext),
    ITagRepositoryAsync
{
}
