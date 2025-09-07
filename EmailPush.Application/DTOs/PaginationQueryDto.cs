namespace EmailPush.Application.DTOs;

/// <summary>
/// Pagination query parameters
/// </summary>
public class PaginationQueryDto
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 10;
    private const int DefaultPageNumber = 1;

    private int _pageNumber = DefaultPageNumber;
    private int _pageSize = DefaultPageSize;

    /// <summary>
    /// Page number (1-based)
    /// </summary>
    /// <example>1</example>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value > 0 ? value : DefaultPageNumber;
    }

    /// <summary>
    /// Number of items per page
    /// </summary>
    /// <example>10</example>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value > 0 ? value : DefaultPageSize);
    }

    /// <summary>
    /// Calculate the number of items to skip
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public int Skip => (PageNumber - 1) * PageSize;
}