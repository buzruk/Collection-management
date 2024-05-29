namespace CollectionManagement.Infrastructure.Repositories;

public class OneTimePasswordRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, OneTimePassword>(dbContext),
    IOneTimePasswordRepositoryAsync
{
}
