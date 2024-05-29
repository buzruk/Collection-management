using CollectionManagement.Domain.Entities;
using CollectionManagement.Shared.DTOs.Tags;

namespace CollectionManagement.Application.Services;

public class TagService(IUnitOfWorkAsync unitOfWork)
  : ITagService
{
  private readonly IUnitOfWorkAsync _unitOfWork = unitOfWork;

  public async Task AddAsync(AddTagDto dto,
                             CancellationToken cancellationToken = default)
  {
    if (dto is null)
      throw new ArgumentNullException(nameof(dto));

    dto.Item.Tags = [];

    foreach (var tag in dto.Tags)
    {
      if (int.TryParse(tag, out var tagId))
      {
        var tagRepository = await _unitOfWork.GetRepositoryAsync<Tag>(cancellationToken);
        var dbTag = await tagRepository.GetAsync(t => t.Id == tagId, cancellationToken: cancellationToken);

        if (dbTag != null) dto.Item.Tags.Add(dbTag);
        continue;
      }
      dto.Item.Tags.Add(new Tag { Name = tag });
    }
  }

  public async Task UpdateAsync(UpdateTagDto dto,
                                CancellationToken cancellationToken = default)
  {
    if (dto is null)
      throw new ArgumentNullException(nameof(dto));

    if (!dto.Tags.Any())
    {
      dto.Item.Tags = [];
      return;
    }
    var updateTagDto = new UpdateTagDto()
    {
      Tags = dto.Tags,
      Item = dto.Item
    };
    await AddAsync(updateTagDto, cancellationToken);
  }
}

