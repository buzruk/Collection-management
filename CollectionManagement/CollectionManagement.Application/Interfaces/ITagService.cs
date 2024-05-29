using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Interfaces;

public interface ITagService
{
    Task AddAsync(IEnumerable<string> tags, Item item, CancellationToken cancellationToken = default);

    Task UpdateAsync(IEnumerable<string> tags, Item item, CancellationToken cancellationToken = default);
}

