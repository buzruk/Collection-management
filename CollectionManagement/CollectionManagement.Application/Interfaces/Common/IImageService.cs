namespace CollectionManagement.Application.Interfaces.Common;

public interface IImageService
{
  Task<string> UploadAsync(IFormFile file, string folder, string domain);

  Task<IEnumerable<string>> UploadAsync(List<IFormFile> files, string folder, string domain);

  Task RemoveAsync(string url, string folder);

  Task RemoveAsync(List<string> urls, string folder);
}

