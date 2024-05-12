namespace CollectionManagement.Infrastructure.Services.Files;

public class FileService
  //(IWebHostEnvironment webHostEnvironment)
  : IFileService
{
  //private readonly string assetsFolder = webHostEnvironment.WebRootPath;

  //public async Task<string> CreateFileAsync(FileDto filemodel, CancellationToken cancellationToken = default)
  //{
  //  string path = Path.Combine("files", Guid.NewGuid().ToString() + ".xlsx");

  //  string fullPath = Path.Combine(assetsFolder, path);

  //  //var stream = new FileStream(fullPath, FileMode.Create);
  //  //await filemodel.File.CopyToAsync(stream);
  //  //stream.Close();

  //  using (FileStream fs =  new FileStream(fullPath, FileMode.Create))
  //  {
  //    await filemodel.File.CopyToAsync(fs);
  //  }

  //  return fullPath;
  //}

  //public async Task<bool> DeleteFileAsync(string path, CancellationToken cancellationToken = default)
  //{
  //  if (File.Exists(path))
  //  {
  //    File.Delete(path);
  //    return true;
  //  }

  //  return false;
  //}

  //public async Task<bool> DeleteImageAsync(string imagePartPath, CancellationToken cancellationToken = default)
  //{
  //  string path = Path.Combine(assetsFolder, imagePartPath);

  //  if (File.Exists(path))
  //  {
  //    try
  //    {
  //      File.Delete(path);
  //      return true;
  //    }
  //    catch
  //    {
  //      return false;
  //    }
  //  }
  //  else return false;
  //}

  //public async Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
  //{
  //  string fileName = ImageHelper.UniqueName(image.FileName);

  //  string partPath = Path.Combine(FileConstants.ResourceImageFolder, fileName);

  //  string path = Path.Combine(assetsFolder, partPath);

  //  //var stream = new FileStream(path, FileMode.Create);
  //  //await image.CopyToAsync(stream);
  //  //stream.Close();

  //  using (FileStream fs =  new FileStream(path, FileMode.Create))
  //  {
  //    await image.CopyToAsync(fs);
  //  }

  //  return partPath;
  //}
  public Task<string> CreateFileAsync(FileDto filemodel, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> DeleteFileAsync(string path, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> DeleteImageAsync(string imagePartPath, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}

