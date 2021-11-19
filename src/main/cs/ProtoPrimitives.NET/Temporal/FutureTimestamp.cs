using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Temporal;

/// <summary>
/// Represents a <see cref="DateTimeOffset"/> in the future, respect to the system time.
/// </summary>
public sealed class FutureTimestamp : AbstractDomainPrimitive<DateTimeOffset>, IComparable<FutureTimestamp>,
    IEquatable<FutureTimestamp>
{
    /// <summary>
    /// Default error message.
    /// </summary>
    public static readonly Message DefaultErrorMessage =
        new("'rawValue' must be in the future respect to the system time.");

    private readonly string _asISOString;

    /// <summary>
    /// Validates input and builds new instance if everything is OK.
    /// </summary>
    /// <param name="rawValue"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="rawValue"/> is not in the future.
    /// </exception>
    public FutureTimestamp(DateTimeOffset rawValue) : this(rawValue, DefaultErrorMessage)
    {
    }

    /// <summary>
    /// Validates input and builds new instance if everything is OK.
    /// </summary>
    /// <param name="rawValue">Must be in the future</param>
    /// <param name="errorMessage">Custom error message</param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="errorMessage"/> is <see langword="null"/>
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When <paramref name="rawValue"/> is not in the future.
    /// </exception>
    public FutureTimestamp(DateTimeOffset rawValue, Message errorMessage) : base(rawValue, errorMessage, (val, msg)
        => Validate(val, msg))
            => _asISOString = Value.ToUniversalTime().ToString(Constants.ToISOStringFormat,
                Constants.ToISOStringFormatInfo);

    private static DateTimeOffset Validate(in DateTimeOffset rawValue, in Message errorMessage)
        => Arguments.GreaterThan(rawValue, DateTimeOffset.UtcNow, nameof(rawValue), errorMessage.Value);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(AbstractDomainPrimitive{TRawType}?)"/>
    public int CompareTo(FutureTimestamp? other) => base.CompareTo(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(AbstractDomainPrimitive{TRawType}?)"/>
    public override bool Equals(object? obj) => Equals(obj as FutureTimestamp);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)"/>
    public bool Equals(FutureTimestamp? other) => base.Equals(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()"/>
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc cref="PastOrPresentTimestamp.ToISOString"/>
    public string ToISOString() => _asISOString;
}
