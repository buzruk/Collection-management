namespace CollectionManagement.Infrastructure.Interfaces.Common;

public interface IAuthService
{
  string GenerateToken(Person person, string role, CancellationToken cancellationToken = default);
}

