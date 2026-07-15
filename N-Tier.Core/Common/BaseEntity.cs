namespace N_Tier.Core.Common;

public abstract class BaseEntity<T>
{
    public T Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedById { get; set; }
    public bool IsDeleted { get; set; }
}