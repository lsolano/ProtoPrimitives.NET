using System.Diagnostics.CodeAnalysis;
using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Numerics;

/// <summary>
/// Valid positive long, meaning <code>&gt;= 1</code> (greater than or equals to one).
/// </summary>
public sealed class PositiveLong : AbstractDomainPrimitive<long>, IEquatable<PositiveLong>,
    IComparable<PositiveLong>
{
    /// <summary>
    /// Error message used when not provided.
    /// </summary>
    public static readonly Message DefaultErrorMessage = new("'rawValue' must be positive.");

    /// <summary>
    /// Error message used when <code>errorMessage</code> parameter is invalid.
    /// </summary>
    public static readonly Message InvalidCustomErrorMessageMessage =
        new("'errorMessage' could not be null, empty or white-space only.");

    /// <summary>
    /// Wraps the raw value and returns a new instance.
    /// </summary>
    /// <param name="rawValue">Must be positive</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="rawValue"/> is zero or negative.
    /// </exception>
    public PositiveLong(long rawValue) : this(rawValue, DefaultErrorMessage)
    {
    }

    /// <summary>
    /// Wraps the raw value and returns a new instance.
    /// </summary>
    /// <param name="rawValue">Must be positive</param>
    /// <param name="errorMessage">Custom error message</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="rawValue"/> is zero or negative.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="errorMessage"/> is <see langword="null"/>.
    /// </exception>
    public PositiveLong(long rawValue, [NotNull] Message errorMessage) :
        base(rawValue, errorMessage, (val, msg) => Validate(val, msg.Value))
    {
    }

    private static long Validate(long rawValue, string errorMessage)
        => Arguments.GreaterThan(rawValue, 0, nameof(rawValue), errorMessage);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(AbstractDomainPrimitive{TRawType}?)" />
    public int CompareTo(PositiveLong? other) => base.CompareTo(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
    public override bool Equals(object? obj) => base.Equals(obj as PositiveLong);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(AbstractDomainPrimitive{TRawType}?)" />
    public bool Equals(PositiveLong? other) => base.Equals(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
    public override int GetHashCode() => base.GetHashCode();
}
