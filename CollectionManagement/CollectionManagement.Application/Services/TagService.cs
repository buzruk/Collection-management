using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services;

public class TagService(IUnitOfWorkAsync unitOfWork)
  : ITagService
{
    private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;

    public async Task AddAsync(IEnumerable<string> tags,
                               Item item,
                               CancellationToken cancellationToken = default)
    {
        item.Tags = [];

        foreach (var tag in tags)
        {
            if (int.TryParse(tag, out var tagId))
            {
                var tagRepository = await _unitOfWork.GetRepositoryAsync<Tag>(cancellationToken);
                var dbTag = await tagRepository.GetAsync(t => t.Id == tagId, cancellationToken: cancellationToken);

                if (dbTag != null) item.Tags.Add(dbTag);
                continue;
            }
            item.Tags.Add(new Tag { Name = tag });
        }
    }

    public async Task UpdateAsync(IEnumerable<string> tags,
                                  Item itemToUpdate,
                                  CancellationToken cancellationToken = default)
    {
        if (!tags.Any())
        {
            itemToUpdate.Tags = [];
            return;
        }
        await AddAsync(tags, itemToUpdate, cancellationToken);
    }
}

