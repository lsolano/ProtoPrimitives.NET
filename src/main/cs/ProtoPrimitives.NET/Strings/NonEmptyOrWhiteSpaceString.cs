using System;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Strings
{
    /// <summary>
    /// Non empty or white-space only string.
    /// </summary>
    public sealed class NonEmptyOrWhiteSpaceString : AbstractDomainPrimitive<string>
    {
        /// <summary>
        /// Comparison strategy used.
        /// </summary>
        public const StringComparison ComparisonStrategy = StringComparison.Ordinal;

        /// <summary>
        /// Default error message.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' can not be empty or white space only.");

        /// <summary>
        /// Error message used when <code>errorMessage</code> parameter is invalid.
        /// </summary>
        public const string InvalidCustomErrorMessageMessage = "'errorMessage' could not be null, empty or white-space only.";

        /// <summary>
        /// Validates input and returns new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
        /// <exception cref="ArgumentNullException">When <paramref name="rawValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">When <paramref name="rawValue"/> is empty or contains only white-spaces.</exception>
        public NonEmptyOrWhiteSpaceString(in string rawValue)
            : base((string?)rawValue, DefaultErrorMessage, (value, msg) => Validate(value, msg)) { }

        /// <summary>
        /// Validates input and returns new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">When <paramref name="rawValue"/> is empty or contains only white-spaces.</exception>
        public NonEmptyOrWhiteSpaceString(in string rawValue, in Message errorMessage)
            : base((string?)rawValue, Arguments.NotNull(errorMessage, nameof(errorMessage), InvalidCustomErrorMessageMessage), (v, m) => Validate(v, m)) {}

        private static string Validate(in string rawValue, in Message errorMessage)
        {
            ConfigurableString.Builder builder = new ConfigurableString.Builder(errorMessage, useSingleMessage: true)
                .WithMinLength(new Numerics.StringLength(1))
                .WithAllowWhiteSpacesOnly(false)
                .WithComparisonStrategy(ComparisonStrategy);

            return builder.Build(rawValue).Value;
        }

        /// <inheritdocs cref="AbstractDomainPrimitive{TRawType}.Equals(IDomainPrimitive{TRawType}?)" />
        public override bool Equals(IDomainPrimitive<string>? other)
        {
            if (other is null) {
                return false;
            }

            return ReferenceEquals(this, other) || Value.Equals(other.Value, ComparisonStrategy);
        }

        /// <inheritdocs cref="AbstractDomainPrimitive{TRawType}.CompareTo(IDomainPrimitive{TRawType}?)" />
        public override int CompareTo(IDomainPrimitive<string>? other)
        {
            if (other is null) {
                return 1;
            }

            return ReferenceEquals(this, other)? 0 : string.Compare(Value, other.Value, ComparisonStrategy);
        }
    }
}
