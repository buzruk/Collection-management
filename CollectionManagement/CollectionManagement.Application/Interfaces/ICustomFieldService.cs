namespace CollectionManagement.Application.Interfaces;

public interface ICustomFieldService
{
    Task<bool> AddAsync(int id, CustomFieldDto customField, CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);
}

