namespace Pharmacy.Application.DTOs.Common;

/// <summary>
/// Logical operators for combining filter conditions
/// </summary>
public enum LogicalOperator
{
    /// <summary>
    /// All conditions must be true (default)
    /// </summary>
    And = 0,
    
    /// <summary>
    /// At least one condition must be true
    /// </summary>
    Or = 1
}
