namespace CollectionManagement.Application.DbContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
  : DbContext(options)
{

  public virtual DbSet<Admin> Admins { get; set; } = default!;
  public virtual DbSet<Collection> Collections { get; set; } = default!;
  public virtual DbSet<Comment> Comments { get; set; } = default!;
  public virtual DbSet<CustomField> CustomFields { get; set; } = default!;
  public virtual DbSet<Item> Items { get; set; } = default!;
  public virtual DbSet<Like> Likes { get; set; } = default!;
  public virtual DbSet<LikeItem> LikeItems { get; set; } = default!;
  public virtual DbSet<Tag> Tags { get; set; } = default!;
  public virtual DbSet<User> Users { get; set; } = default!;

  //protected override void OnModelCreating(ModelBuilder modelBuilder)
  //{
  //  base.OnModelCreating(modelBuilder);
  //  //modelBuilder.ApplyConfiguration(new SuperAdminConfiguration());
  //}
}
