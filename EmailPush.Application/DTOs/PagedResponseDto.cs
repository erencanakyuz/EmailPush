using System.Text.Json.Serialization;

namespace EmailPush.Application.DTOs;

/// <summary>
/// Generic paged response DTO
/// </summary>
/// <typeparam name="T">Type of data in the response</typeparam>
public class PagedResponseDto<T>
{
    /// <summary>
    /// Collection of data items
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;

    /// <summary>
    /// Indicates if there's a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there's a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Creates a new PagedResponseDto instance
    /// </summary>
    /// <param name="data">Collection of data items</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="totalItems">Total number of items</param>
    public PagedResponseDto(IEnumerable<T> data, int pageNumber, int pageSize, int totalItems)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
    }

    /// <summary>
    /// Default constructor for serialization
    /// </summary>
    public PagedResponseDto()
    {
    }
}