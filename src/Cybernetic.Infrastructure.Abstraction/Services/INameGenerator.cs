namespace Cybernetic.Infrastructure.Abstraction.Services;

/// <summary>
/// Service to generate names.
/// </summary>
public interface INameGenerator
{
    /// <summary>
    /// Generates name.
    /// </summary>
    /// <param name="length">Length of the name.</param>
    /// <returns>Generated name.</returns>
    string Generate(int length);
}