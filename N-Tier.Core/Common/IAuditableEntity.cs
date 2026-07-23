namespace N_Tier.Core.Common;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; set; }
    int? CreatedById { get; set; }
    int? UpdatedById { get; set; }
}