using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services;

public class CustomFieldService(IUnitOfWorkAsync unitOfWork)
  : ICustomFieldService
{
    private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;

    public async Task<bool> AddAsync(int id,
                                     CustomFieldDto customFieldDto,
                                     CancellationToken cancellationToken = default)
    {
        var customFieldRepository = await _unitOfWork.GetRepositoryAsync<CustomField>();
        var customField = await customFieldRepository.GetAsync(c => c.Id == id);

        if (customField == null)
        {
            var entity = new CustomField
            {
                Name = customFieldDto.Name,
                Type = customFieldDto.Type
            };

            await customFieldRepository.AddAsync(entity);

            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        else throw new StatusCodeException(HttpStatusCode.BadRequest, "CustomFieldCollection is not created or already exists");
    }

    public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid custom field ID");
        }

        var customFieldRepository = await _unitOfWork.GetRepositoryAsync<CustomField>();
        var customField = await customFieldRepository.GetAsync(c => c.Id == id);

        if (customField == null)
        {
            throw new StatusCodeException(HttpStatusCode.NotFound, "Custom field not found");
        }

        await customFieldRepository.RemoveAsync(c => c.Id == id);

        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}

