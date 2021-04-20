using System;
using Triplex.Validations;

namespace ProtoPrimitives.NET.Exceptions
{
    /// <summary>
    /// Non-empty or null exception message.
    /// </summary>
    public sealed class Message : IDomainPrimitive<string>
    {
        /// <summary>
        /// Validates input and returns new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
        /// <exception cref="ArgumentNullException">When <paramref name="rawValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">When <paramref name="rawValue"/> is empty or contains only white-spaces.</exception>
        public Message(in string rawValue)
        {
            Arguments.NotNull(rawValue, nameof(rawValue));

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                throw new FormatException("Cant be empty or white-space only.");
            }

            Value = rawValue;
        }

        /// <summary>
        /// Wrapped value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Same as wrapped value comparison.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable<IDomainPrimitive<string>>.CompareTo(IDomainPrimitive<string>? other) => CompareTo(other as Message);

        /// <summary>
        /// Compares with <see cref="StringComparison.Ordinal"/> strategy both wrapped values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(in Message? other) => string.CompareOrdinal(Value, other?.Value);

        /// <summary>
        /// Same as wrapped value equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IDomainPrimitive<string>? other) => Value.Equals(other?.Value, StringComparison.Ordinal);
    }
}
