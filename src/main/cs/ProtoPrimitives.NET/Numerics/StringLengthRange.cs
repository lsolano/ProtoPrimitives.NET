using System.Diagnostics.CodeAnalysis;

namespace Triplex.ProtoDomainPrimitives.Numerics;

/// <summary>
/// Defines a length's range for strings.
/// </summary>
public sealed class StringLengthRange
{
    /// <summary>
    /// Validates that the given input conform a valid range and sets properties.
    /// </summary>
    /// <param name="min">Can be equals to <paramref name="max"/> but not <see langword="null"/></param>
    /// <param name="max">Can be equals to <paramref name="min"/> but not <see langword="null"/></param>
    public StringLengthRange([NotNull] StringLength min, [NotNull] StringLength max)
    {
        Validate(min, max);

        (Min, Max) = (min, max);
    }

    /// <summary>
    /// Min length (inclusive)
    /// </summary>
    [NotNull]
    public StringLength Min { get; }

    /// <summary>
    /// Max length (inclusive)
    /// </summary>
    [NotNull]
    public StringLength Max { get; }

    internal static void Validate([NotNull] StringLength min, [NotNull] StringLength max)
    {
        Arguments.NotNull(min, nameof(min));
        Arguments.NotNull(max, nameof(max));
        Arguments.LessThanOrEqualTo(min, max, nameof(min), 
            $"{nameof(min)} must be less than or equals to (<=) {nameof(max)} ({max}).");
    }
}
