namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the status of a sale
/// </summary>
public enum SaleStatus
{
    /// <summary>
    /// Unknown status - used for validation purposes
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Sale is active and not cancelled
    /// </summary>
    Active,
    
    /// <summary>
    /// Sale has been cancelled
    /// </summary>
    Cancelled
}

