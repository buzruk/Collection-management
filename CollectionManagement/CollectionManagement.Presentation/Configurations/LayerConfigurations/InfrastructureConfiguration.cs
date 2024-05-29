using CollectionManagement.Infrastructure.DbContexts;

namespace CollectionManagement.Presentation.Configurations.LayerConfigurations;

public static class InfrastructureConfiguration
{
  public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(
      options => options.UseSqlite(configuration.GetConnectionString("AppDb"))
    );

    services.AddDbContext<AppIdentityDbContext>(
      options => options.UseSqlite(configuration.GetConnectionString("IdentityDb"))
    );

  }
}
