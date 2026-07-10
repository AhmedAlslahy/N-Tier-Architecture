using N_Tier.Core.Identity;

namespace N_Tier.DataAccess.Persistence;

public class SarhneDbContext : DbContext
{
    public SarhneDbContext(DbContextOptions<SarhneDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SarhneDbContext).Assembly);
    }
}