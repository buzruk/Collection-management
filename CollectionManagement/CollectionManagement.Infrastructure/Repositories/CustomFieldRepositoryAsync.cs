namespace CollectionManagement.Infrastructure.Repositories;

public class CustomFieldRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, CustomField>(dbContext),
    ICustomFieldRepositoryAsync
{
}
