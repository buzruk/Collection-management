namespace CollectionManagement.Infrastructure.Interfaces.Files;

public interface IFileService
{
  Task<string> CreateFileAsync(FileDto filemodel, CancellationToken cancellationToken = default);

  Task<bool> DeleteFileAsync(string path, CancellationToken cancellationToken = default);

  Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default);

  Task<bool> DeleteImageAsync(string imagePartPath, CancellationToken cancellationToken = default);
}

