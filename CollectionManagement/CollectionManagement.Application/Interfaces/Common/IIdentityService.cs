namespace CollectionManagement.Application.Interfaces.Common;

public interface IIdentityService
{
  string Id { get; }

  string UserName { get; }

  string ImagePath { get; }
}

