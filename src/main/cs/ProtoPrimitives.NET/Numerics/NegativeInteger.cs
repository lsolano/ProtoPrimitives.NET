using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Numerics;

/// <summary>
/// Valid negative integer, meaning <code>&lt; 0</code> (less than zero).
/// </summary>
public sealed class NegativeInteger : AbstractDomainPrimitive<int>, IEquatable<NegativeInteger>,
    IComparable<NegativeInteger>
{
    /// <summary>
    /// Error message used when not provided.
    /// </summary>
    public static readonly Message DefaultErrorMessage = new("'rawValue' must be negative.");

    /// <summary>
    /// Error message used when <code>errorMessage</code> parameter is invalid.
    /// </summary>
    public static readonly Message InvalidCustomErrorMessageMessage =
        new("'errorMessage' could not be null, empty or white-space only.");

    /// <summary>
    /// Wraps the raw value and returns a new instance.
    /// </summary>
    /// <param name="rawValue">Must be negative</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="rawValue"/> is zero or positive.
    /// </exception>
    public NegativeInteger(int rawValue) : this(rawValue, DefaultErrorMessage)
    {
    }

    /// <summary>
    /// Wraps the raw value and returns a new instance.
    /// </summary>
    /// <param name="rawValue">Must be positive</param>
    /// <param name="errorMessage">Custom error message</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="rawValue"/> is zero or positive.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="errorMessage"/> is <see langword="null"/>.
    /// </exception>
    public NegativeInteger(int rawValue, Message errorMessage) :
        base(rawValue, errorMessage, (val, msg) => Validate(val, msg))
    {
    }

    private static int Validate(int rawValue, Message errorMessage)
        => Arguments.LessThan(rawValue, 0, nameof(rawValue), errorMessage.Value);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(AbstractDomainPrimitive{TRawType}?)" />
    public int CompareTo(NegativeInteger? other) => base.CompareTo(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
    public override bool Equals(object? obj) => base.Equals(obj as NegativeInteger);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(AbstractDomainPrimitive{TRawType}?)" />
    public bool Equals(NegativeInteger? other) => base.Equals(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
    public override int GetHashCode() => base.GetHashCode();
}
