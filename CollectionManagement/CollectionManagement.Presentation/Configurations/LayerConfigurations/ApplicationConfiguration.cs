namespace CollectionManagement.Presentation.Configurations.LayerConfigurations;
public static class ApplicationConfiguration
{
  public static void ConfigureServices(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
  {
    services.AddTransient<IAuthService, AuthService>();
    services.AddTransient<IUnitOfWorkAsync, UnitOfWorkAsync<AppDbContext>>();
    services.AddTransient<IImageService, ImageService>();
    services.AddTransient<IIdentityService, IdentityService>();
    services.AddTransient<ICollectionService, CollectionService>();
    services.AddTransient<ICommentService, CommentService>();
    services.AddTransient<ICustomFieldService, CustomFieldService>();
    services.AddTransient<IItemService, ItemService>();
    services.AddTransient<ILikeService, LikeService>();
    services.AddTransient<ITagService, TagService>();
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<IOneTimePasswordService, OneTimePasswordService>();

    //services.AddMemoryCache();
  }
}

