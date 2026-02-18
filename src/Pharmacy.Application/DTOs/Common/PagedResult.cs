namespace Pharmacy.Application.DTOs.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }

    public static PagedResult<T> Create(IEnumerable<T> data, int totalRecords, int pageNumber, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        
        return new PagedResult<T>
        {
            Data = data,
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasNextPage = pageNumber < totalPages,
            HasPreviousPage = pageNumber > 1
        };
    }
}