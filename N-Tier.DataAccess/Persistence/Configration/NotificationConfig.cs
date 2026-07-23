namespace N_Tier.DataAccess.Persistence.Configration;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}