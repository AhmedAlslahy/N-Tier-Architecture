namespace N_Tier.DataAccess.Persistence.Configration;

public class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(M => M.Id);

        builder.Property(m => m.Content)
            .HasMaxLength(200);

        builder.Property(m => m.PhotoUrl)
            .HasMaxLength(500);
    }
}