namespace CollectionManagement.Infrastructure.Interfaces.Common;

public interface IIdentityService
{
  int? Id { get; }

  string UserName { get; }

  string ImagePath { get; }
}

