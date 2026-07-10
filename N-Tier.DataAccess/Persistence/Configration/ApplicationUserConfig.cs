using N_Tier.Core.Identity;

namespace N_Tier.DataAccess.Persistence.Configration;

public class ApplicationUserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(u => u.FullName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(u => u.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(u => u.PublicLink)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(u => u.ProfileDescription)
            .HasMaxLength(300)
            .IsRequired(false);

        builder.Property(u => u.LastSeen)
            .IsRequired(false);

        builder.Property(u => u.ProfileViewsCount)
            .HasDefaultValue(0);

        //Relations
        //User Setting
        builder.HasOne(u => u.UserSetting)
         .WithOne(s => s.User)
         .HasForeignKey<UserSetting>(s => s.UserId)
         .IsRequired();

        //Message
        builder.HasMany(u => u.ReceivedMessages)
          .WithOne(m => m.Receiver)
          .HasForeignKey(m => m.ReceiverId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SentMessages)
          .WithOne(m => m.Sender)
          .HasForeignKey(m => m.SenderId)
          .OnDelete(DeleteBehavior.Restrict);

        //Notification
        builder.HasMany(u => u.Notifications)
          .WithOne(n => n.Receiver)
          .HasForeignKey(n => n.ReceiverId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SentMessages)
          .WithOne(n => n.Sender)
          .HasForeignKey(m => m.SenderId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
             .UsingEntity<Dictionary<string, object>>(
            "UserRole",
            j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
            j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));
    }
}