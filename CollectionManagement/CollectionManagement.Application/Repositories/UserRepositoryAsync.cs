namespace CollectionManagement.Application.Repositories;

public class UserRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, User>(dbContext),
    IUserRepositoryAsync
{
}
