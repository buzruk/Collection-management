namespace CollectionManagement.Infrastructure.Interfaces.Common;

public interface IImageService
{
  Task<string> SaveImageAsync(IFormFile file, CancellationToken cancellationToken = default);

  Task<bool> DeleteImageAsync(string imagePath, CancellationToken cancellationToken = default);
}

