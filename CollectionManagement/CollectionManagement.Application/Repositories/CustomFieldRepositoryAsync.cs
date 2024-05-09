namespace CollectionManagement.Application.Repositories;

public class CustomFieldRepositoryAsync(AppDbContext dbContext) 
  : GenericRepositoryAsync<AppDbContext, CustomField>(dbContext),
    ICustomFieldRepositoryAsync
{
}
