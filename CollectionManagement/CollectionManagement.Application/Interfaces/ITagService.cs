using CollectionManagement.Shared.DTOs.Tags;

namespace CollectionManagement.Application.Interfaces;

public interface ITagService
{
  Task AddAsync(AddTagDto dto, CancellationToken cancellationToken = default);

  Task UpdateAsync(UpdateTagDto dto, CancellationToken cancellationToken = default);
}

