namespace CollectionManagement.Presentation.Configurations.LayerConfigurations;
public static class InfrastructureConfiguration
{
  public static void ConfigureServices(this IServiceCollection services)
  {
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IAdminService, AdminService>();
    services.AddScoped<IAdminUserService, AdminUserService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync<AppDbContext>>();
    services.AddScoped<IImageService, ImageService>();
    services.AddScoped<IIdentityService, IdentityService>();
    services.AddScoped<IFileService, FileService>();
    services.AddScoped<ICollectionService, CollectionService>();
    services.AddScoped<ICommentService, CommentService>();
    services.AddScoped<ICustomFieldService, CustomFieldService>();
    services.AddScoped<ItemService, ItemService>();
    services.AddScoped<ILikeService, LikeService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<IUserService, UserService>();

    services.AddMemoryCache();
    services.AddHttpContextAccessor();
  }
}

