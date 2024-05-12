namespace CollectionManagement.Infrastructure.Services.Tags;

public class TagService(IUnitOfWorkAsync unitOfWork) 
  : ITagService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;

  public async Task CreateTagAsync(IEnumerable<string> tags, Item item, CancellationToken cancellationToken = default)
  {
    item.Tags = [];

    foreach (var tag in tags)
    {
      if (int.TryParse(tag, out var tagId))
      {
        var tagRepository = await _unitOfWork.GetRepositoryAsync<Tag>();
        var dbTag = await tagRepository.GetAsync(t => t.Id == tagId);

        if (dbTag != null) item.Tags.Add(dbTag);
        continue;
      }
      item.Tags.Add(new Tag { Name = tag });
    }
  }

  public async Task UpdateTagAsync(IEnumerable<string> tags, Item itemToUpdate, CancellationToken cancellationToken = default)
  {
    if (!tags.Any())
    {
      itemToUpdate.Tags = [];
      return;
    }
    await CreateTagAsync(tags, itemToUpdate);
  }
}

