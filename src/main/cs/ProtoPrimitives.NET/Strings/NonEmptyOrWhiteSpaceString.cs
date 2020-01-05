using ProtoPrimitives.NET.Exceptions;
using System;
using Triplex.Validations;

namespace ProtoPrimitives.NET.Strings
{
    /// <summary>
    /// Non empty or white-space only string.
    /// </summary>
    public sealed class NonEmptyOrWhiteSpaceString : IDomainPrimitive<string>
    {
        /// <summary>
        /// Comparison strategy used.
        /// </summary>
        public const StringComparison ComparisonStrategy = StringComparison.Ordinal;

        /// <summary>
        /// Default error message.
        /// </summary>
        public const string DefaultErrorMessage = "'rawValue' can not be empty or white space only.";

        /// <summary>
        /// Error message used when <code>errorMessage</code> parameter is invalid.
        /// </summary>
        public const string InvalidCustomErrorMessageMessage = "'errorMessage' could not be null, empty or white-space only.";

        private readonly ConfigurableString _value;

        /// <summary>
        /// Validates input and returns new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
        /// <exception cref="ArgumentNullException">When <paramref name="rawValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">When <paramref name="rawValue"/> is empty or contains only white-spaces.</exception>
        public NonEmptyOrWhiteSpaceString(string rawValue) => _value = Validate(rawValue, DefaultErrorMessage);

        /// <summary>
        /// Validates input and returns new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">When <paramref name="rawValue"/> is empty or contains only white-spaces.</exception>
        public NonEmptyOrWhiteSpaceString(string rawValue, string errorMessage) => _value = Validate(rawValue, errorMessage);

        private static ConfigurableString Validate(string rawValue, string errorMessage)
        {
            Arguments.NotNull(errorMessage, nameof(errorMessage), InvalidCustomErrorMessageMessage);

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new FormatException(InvalidCustomErrorMessageMessage);
            }

            ConfigurableString.Builder builder = new ConfigurableString.Builder(new Message(errorMessage), useSingleMessage: true)
                .WithMinLength(new Numerics.StringLength(1))
                .WithAllowWhiteSpacesOnly(false)
                .WithComparisonStrategy(ComparisonStrategy);

            return builder.Build(rawValue);
        }

        ///<inheritdoc cref="IDomainPrimitive{TRawType}.Value"/>
        public string Value => _value.Value;

        string IDomainPrimitive<string>.Value => _value.Value;

        /// <summary>
        /// Same as wrapped value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>
        /// Same as wrapped value.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _value.ToString();

        int IComparable<IDomainPrimitive<string>>.CompareTo(IDomainPrimitive<string> other)
            => CompareTo(other as NonEmptyOrWhiteSpaceString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(NonEmptyOrWhiteSpaceString? other)
            => _value.CompareTo(other);

        /// <summary>
        /// Same as wrapped value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
            => Equals(obj as NonEmptyOrWhiteSpaceString);

        bool IEquatable<IDomainPrimitive<string>>.Equals(IDomainPrimitive<string> other)
            => Equals(other as NonEmptyOrWhiteSpaceString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(NonEmptyOrWhiteSpaceString? other) => _value.Equals(other);
    }
}
