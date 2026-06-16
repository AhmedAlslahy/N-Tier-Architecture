namespace N_Tier.DataAccess.Persistence;

public class SarhneDbContext : IdentityDbContext<User>
{
    public SarhneDbContext(DbContextOptions<SarhneDbContext> options) : base(options)
    {
    }

    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SarhneDbContext).Assembly);
    }
}