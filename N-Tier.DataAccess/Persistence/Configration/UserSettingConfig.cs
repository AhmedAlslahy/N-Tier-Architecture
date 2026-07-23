namespace N_Tier.DataAccess.Persistence.Configration;

public class UserSettingConfig : IEntityTypeConfiguration<UserSetting>
{
    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}