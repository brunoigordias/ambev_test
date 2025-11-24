namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product rating.
/// This is an owned entity that represents the rating information for a product.
/// </summary>
public class Rating
{
    /// <summary>
    /// Gets or sets the average rating value
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the number of ratings
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Initializes a new instance of the Rating class
    /// </summary>
    public Rating()
    {
        Rate = 0;
        Count = 0;
    }

    /// <summary>
    /// Initializes a new instance of the Rating class with specified values
    /// </summary>
    public Rating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }
}


