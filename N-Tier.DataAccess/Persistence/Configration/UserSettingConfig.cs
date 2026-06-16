namespace N_Tier.DataAccess.Persistence.Configration;

public class UserSettingConfig : IEntityTypeConfiguration<UserSetting>
{
    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.ToTable("UserSettings");

        builder.HasKey(x => x.Id);
    }
}