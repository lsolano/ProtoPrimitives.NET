using System;

using Triplex.Validations;
using Triplex.Validations.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Exceptions
{
    /// <summary>
    /// Non-empty or null exception message. Comparison other <see cref="String"/>-related operation are done using <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    public sealed class Message : IDomainPrimitive<string>
    {
        /// <summary>
        /// Validates input and returns new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
        /// <exception cref="ArgumentNullException">When <paramref name="rawValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentFormatException">When <paramref name="rawValue"/> is empty or contains only white-spaces.</exception>
        public Message(in string rawValue)
            => Value = Arguments.NotNullEmptyOrWhiteSpaceOnly(rawValue, nameof(rawValue));

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
        public int CompareTo(in Message? other) => string.Compare(Value, other?.Value, StringComparison.Ordinal);

        /// <summary>
        /// Same as wrapped value equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IDomainPrimitive<string>? other) => Value.Equals(other?.Value, StringComparison.Ordinal);

        /// <summary>
        /// Same as <see cref="Equals(IDomainPrimitive{string}?)"/> after casting the argument.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => Equals(obj as IDomainPrimitive<string>);

        /// <summary>
        /// Gets hashcode based on <see cref="Value"/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

        /// <summary>
        /// Same as <see cref="Value"/>
        /// </summary>
        /// <returns></returns>
        public override string? ToString() => Value;

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in Message left, in Message right) => !(left == right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in Message left, in Message right)
        {
            if (left is null) {
                return right is null;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(in Message left, in Message right) {
            if (left is null) {
                return right is not null;
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(in Message left, in Message right) {
            if (left is null) {
                return true;
            }

            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(in Message left, in Message right) {
            if (left is null) {
                return false;
                
            } else if (right is null) {
                return true;
            }

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(in Message left, in Message right) {
            if (right is null) {
                return true;

            } else if (left is null) {
                return false;
            }

            return left.CompareTo(right) >= 0;
        }
    }
}
