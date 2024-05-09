namespace CollectionManagement.Application.Repositories;

public class AdminRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, Admin>(dbContext),
    IAdminRepositoryAsync
{
}
