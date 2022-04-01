using System.Diagnostics.CodeAnalysis;
using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Strings;

/// <summary>
/// Non empty or white-space only string.
/// </summary>
public sealed class NonEmptyOrWhiteSpaceString : AbstractDomainPrimitive<string>,
    IEquatable<NonEmptyOrWhiteSpaceString>, IComparable<NonEmptyOrWhiteSpaceString>
{
    /// <summary>
    /// Comparison strategy used.
    /// </summary>
    public static readonly StringComparison ComparisonStrategy = StringComparison.Ordinal;

    /// <summary>
    /// Default error message.
    /// </summary>
    public static readonly Message DefaultErrorMessage = new("'rawValue' can not be empty or white space only.");

    /// <summary>
    /// Error message used when <code>errorMessage</code> parameter is invalid.
    /// </summary>
    public static readonly string InvalidCustomErrorMessageMessage =
        "'errorMessage' could not be null, empty or white-space only.";

    /// <summary>
    /// Validates input and returns new instance if everything is OK.
    /// </summary>
    /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="rawValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="FormatException">
    /// When <paramref name="rawValue"/> is empty or contains only white-spaces.
    /// </exception>
    public NonEmptyOrWhiteSpaceString([NotNull] string? rawValue)
        : base(rawValue, DefaultErrorMessage, (value, msg) => Validate(value, msg)) { }

    /// <summary>
    /// Validates input and returns new instance if everything is OK.
    /// </summary>
    /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
    /// <param name="errorMessage">Custom error message</param>
    /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/>.</exception>
    /// <exception cref="FormatException">
    /// When <paramref name="rawValue"/> is empty or contains only white-spaces.
    /// </exception>
    public NonEmptyOrWhiteSpaceString([NotNull] string? rawValue, [NotNull] Message? errorMessage)
        : base(rawValue, Arguments.NotNull(errorMessage, nameof(errorMessage), InvalidCustomErrorMessageMessage),
            (v, m) => Validate(v, m))
    { }

    [return: NotNull] 
    private static string Validate(string? rawValue, Message errorMessage)
    {
        ConfigurableString.Builder builder = new ConfigurableString.Builder(errorMessage, useSingleMessage: true)
            .WithMinLength(new Numerics.StringLength(1))
            .WithAllowWhiteSpacesOnly(false)
            .WithComparisonStrategy(ComparisonStrategy);

        return builder.Build(rawValue).Value;
    }

    /// <summary>
    /// Categorizes based on value and <see cref="ComparisonStrategy"/>.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => string.GetHashCode(Value, ComparisonStrategy);

    /// <summary>
    /// Same as <see cref="Equals(NonEmptyOrWhiteSpaceString?)"/> after casting <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as NonEmptyOrWhiteSpaceString);

    /// <inheritdocs cref="AbstractDomainPrimitive{TRawType}.Equals(AbstractDomainPrimitive{TRawType}?)" />
    public bool Equals(NonEmptyOrWhiteSpaceString? other)
        => RelationalOperatorsOverloadHelper.SelfIsEqualsTo(this, other,
            o => Value.Equals(o.Value, ComparisonStrategy));

    /// <inheritdocs cref="AbstractDomainPrimitive{TRawType}.CompareTo(AbstractDomainPrimitive{TRawType}?)" />
    public int CompareTo(NonEmptyOrWhiteSpaceString? other)
        => RelationalOperatorsOverloadHelper.SelfComparedToOther(this, other,
            o => string.Compare(Value, o.Value, ComparisonStrategy));
}
