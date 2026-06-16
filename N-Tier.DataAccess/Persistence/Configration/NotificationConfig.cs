namespace N_Tier.DataAccess.Persistence.Configration;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
            .HasMaxLength(50);

        builder.Property(n => n.Body)
            .HasMaxLength(100);
    }
}