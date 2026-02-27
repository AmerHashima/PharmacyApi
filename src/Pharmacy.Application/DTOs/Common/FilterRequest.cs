namespace Pharmacy.Application.DTOs.Common;

/// <summary>
/// Represents a single filter condition
/// </summary>
public class FilterRequest
{
    /// <summary>
    /// Property name to filter on
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Filter value
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Filter operation (Equal, Contains, etc.)
    /// </summary>
    public FilterOperation Operation { get; set; } = FilterOperation.Equal;

    /// <summary>
    /// Logical operator to combine with next filter (default: AND)
    /// </summary>
    public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;

    /// <summary>
    /// Group number for complex filtering (filters with same group are evaluated together)
    /// Leave null for simple AND/OR filtering
    /// </summary>
    public int? GroupId { get; set; }
}

public enum FilterOperation
{
    Equal = 0,
    NotEqual = 1,
    Contains = 2,
    StartsWith = 3,
    EndsWith = 4,
    GreaterThan = 5,
    LessThan = 6,
    GreaterThanOrEqual = 7,
    LessThanOrEqual = 8,
    In = 9,
    NotIn = 10,
    IsNull = 11,
    IsNotNull = 12
}