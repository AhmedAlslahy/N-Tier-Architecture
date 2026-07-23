using N_Tier.Core.Entities.Identity;

namespace N_Tier.DataAccess.Persistence.Configration;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}