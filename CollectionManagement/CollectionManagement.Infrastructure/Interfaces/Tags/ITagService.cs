namespace CollectionManagement.Infrastructure.Interfaces.Tags;

public interface ITagService
{
  Task CreateTagAsync(IEnumerable<string> tags, Item item, CancellationToken cancellationToken = default);

  Task UpdateTagAsync(IEnumerable<string> tags, Item itemToUpdate, CancellationToken cancellationToken = default);
}

