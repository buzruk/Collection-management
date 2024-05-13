using Microsoft.Extensions.Options;

namespace CollectionManagement.Presentation.Configurations.LayerConfigurations;

public static class ApplicationConfiguration
{
  public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    services.AddDbContext<AppDbContext>(
      options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))
    );

    services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync<AppDbContext>>();
  }
}
