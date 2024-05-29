using CollectionManagement.Application.Interfaces;
using CollectionManagement.Application.Services;

namespace CollectionManagement.Presentation.Configurations.LayerConfigurations;
public static class ApplicationConfiguration
{
  public static void ConfigureServices(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
  {
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync<AppDbContext>>();
    services.AddScoped<IImageService, ImageService>();
    services.AddScoped<IIdentityService, IdentityService>();
    services.AddScoped<ICollectionService, CollectionService>();
    services.AddScoped<ICommentService, CommentService>();
    services.AddScoped<ICustomFieldService, CustomFieldService>();
    services.AddScoped<IItemService, ItemService>();
    services.AddScoped<ILikeService, LikeService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IOneTimePasswordService, OneTimePasswordService>();



    //services.AddSingleton<IWebHostBuilder, WebHostBuilder>();

    services.AddMemoryCache();
    //services.AddHttpContextAccessor();
  }
}

