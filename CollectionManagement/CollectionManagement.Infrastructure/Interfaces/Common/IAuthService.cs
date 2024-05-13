namespace CollectionManagement.Infrastructure.Interfaces.Common;

public interface IAuthService
{
  string GenerateToken(User person, string role);
}

