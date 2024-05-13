namespace CollectionManagement.Presentation.Configurations;

public static class WebConfiguration
{
  public static void ConfigureWeb(this IServiceCollection services, IConfiguration configuration)
  {
    services.ConfigureAuth(configuration);
  }
}

