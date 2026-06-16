namespace N_Tier.Application.Abstraction;

public record Error(string Code, string Description, ErrorType? StatusCode)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
}