namespace N_Tier.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}