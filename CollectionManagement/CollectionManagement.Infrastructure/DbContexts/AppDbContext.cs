namespace CollectionManagement.Infrastructure.DbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
  : IdentityDbContext<User>(options)
{

  public virtual DbSet<Collection> Collections { 
    get; 
    set; 
  }

  public virtual DbSet<Comment> Comments { 
    get; 
    set; 
  }

  public virtual DbSet<CustomField> CustomFields { 
    get; 
    set; 
  }

  public virtual DbSet<Item> Items { 
    get; 
    set; 
  }

  public virtual DbSet<Like> Likes { 
    get; 
    set; 
  }

  public virtual DbSet<LikeItem> LikeItems { 
    get; 
    set; 
  }

  public virtual DbSet<Tag> Tags { 
    get; 
    set; 
  }

  public DbSet<OneTimePassword> OtpModels { 
    get; 
    set; 
  }

  //protected override void OnModelCreating(ModelBuilder modelBuilder)
  //{
  //  base.OnModelCreating(modelBuilder);
  //  //modelBuilder.ApplyConfiguration(new SuperAdminConfiguration());
  //}
}
