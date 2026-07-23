namespace N_Tier.Core.Common;

public abstract class BaseEntity<T> : IBaseEntity<T>
{
    public T Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedById { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedById { get; set; }
    public int? UpdatedById { get; set; }
}

public abstract class BaseEntity : BaseEntity<int>, IBaseEntity;

public interface IBaseEntity<T> : IAuditableEntity, ISoftDeletableEntity
{
    T Id { get; set; }
}

public interface IBaseEntity : IBaseEntity<int>;