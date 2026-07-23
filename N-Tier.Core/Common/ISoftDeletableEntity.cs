namespace N_Tier.Core.Common;

public interface ISoftDeletableEntity
{
    DateTime? DeletedAt { get; protected set; }
    int? DeletedById { get; set; }
    bool IsDeleted { get; protected set; }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}