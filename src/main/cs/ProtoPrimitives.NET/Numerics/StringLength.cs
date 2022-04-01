using System.Diagnostics.CodeAnalysis;
using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Numerics;

/// <summary>
/// Represents a valid string length, meaning from zero (0) to <see cref="int.MaxValue"/>
/// </summary>
public sealed class StringLength : AbstractDomainPrimitive<int>, IEquatable<StringLength>, IComparable<StringLength>
{
    /// <summary>
    /// Default error message if the provided value is not valid.
    /// </summary>
    public static readonly Message DefaultErrorMessage = new("'rawValue' must be zero or positive.");

    /// <summary>
    /// Minimum allowed.
    /// </summary>
    public static readonly StringLength Min = new(0);

    /// <summary>
    /// Maximum allowed.
    /// </summary>
    public static readonly StringLength Max = new(int.MaxValue);

    /// <summary>
    /// Initializes an instance after validating provided input.
    /// </summary>
    /// <param name="rawValue"></param>
    public StringLength(int rawValue) : this(rawValue, DefaultErrorMessage)
    {
    }

    /// <summary>
    /// Initializes an instance after validating provided input.
    /// </summary>
    /// <param name="rawValue"></param>
    /// <param name="errorMessage">Can not be <see langword="null"/></param>
    public StringLength(int rawValue, [NotNull] Message errorMessage) :
        base(rawValue, errorMessage, (val, msg) => Validate(val, msg.Value))
    {
    }

    private static int Validate(int rawValue, string errorMessage)
        => Arguments.GreaterThanOrEqualTo(rawValue, 0, nameof(rawValue), errorMessage);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(AbstractDomainPrimitive{TRawType}?)" />
    public int CompareTo(StringLength? other) => base.CompareTo(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
    public override bool Equals(object? obj) => base.Equals(obj as StringLength);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(AbstractDomainPrimitive{TRawType}?)" />
    public bool Equals(StringLength? other) => base.Equals(other);

    /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
    public override int GetHashCode() => base.GetHashCode();
}
