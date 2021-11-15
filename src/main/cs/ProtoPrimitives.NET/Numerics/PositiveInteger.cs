using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Numerics;

/// <summary>
/// Valid positive integer, meaning <code>&gt;= 1</code> (greater than or equals to one).
/// </summary>
public sealed class PositiveInteger : AbstractDomainPrimitive<int>, IEquatable<PositiveInteger>,
    IComparable<PositiveInteger>
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
    public PositiveInteger(in int rawValue) : this(rawValue, DefaultErrorMessage)
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
    public PositiveInteger(in int rawValue, in Message errorMessage) :
        base(rawValue, errorMessage, (val, msg) => Validate(val, msg))
    {
    }

    private static int Validate(in int rawValue, in Message errorMessage)
        => Arguments.GreaterThan(rawValue, 0, nameof(rawValue), errorMessage.Value);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(AbstractDomainPrimitive{TRawType}?)" />
    public int CompareTo(PositiveInteger? other) => base.CompareTo(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
    public override bool Equals(object? obj) => base.Equals(obj as PositiveInteger);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(AbstractDomainPrimitive{TRawType}?)" />
    public bool Equals(PositiveInteger? other) => base.Equals(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
    public override int GetHashCode() => base.GetHashCode();
}
