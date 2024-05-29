using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CollectionManagement.Infrastructure.DbContexts;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
  : IdentityDbContext<User, IdentityRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
