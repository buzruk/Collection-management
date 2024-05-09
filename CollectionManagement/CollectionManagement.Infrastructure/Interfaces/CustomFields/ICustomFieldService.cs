namespace CollectionManagement.Infrastructure.Interfaces.CustomFields;

public interface ICustomFieldService
{
  Task<bool> CreateCustomFieldAsync(int id, CustomFieldDto customField, CancellationToken cancellationToken = default);

  Task<bool> DeleteCustomFieldAsync(int id, CancellationToken cancellationToken = default);
}

