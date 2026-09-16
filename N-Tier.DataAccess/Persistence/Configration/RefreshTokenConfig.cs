using N_Tier.Core.Entities.Identity;

namespace N_Tier.DataAccess.Persistence.Configration;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.Property(x => x.TokenHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.ExpiresOn)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.RevokedOn)
            .IsRequired(false);

        builder.Property(x => x.ReplacedByTokenHash)
           .HasMaxLength(500)
           .IsRequired(false);

        //user
        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}