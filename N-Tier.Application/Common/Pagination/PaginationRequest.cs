namespace N_Tier.Application.Common.Pagination;

public class PaginationRequest
{
    private const int MaxPageSize = 100;

    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value <= 0)
                _pageSize = 10;
            else
                _pageSize = Math.Min(value, MaxPageSize);
        }
    }
}