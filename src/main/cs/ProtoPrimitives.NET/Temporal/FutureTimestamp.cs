using System;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Temporal
{
    /// <summary>
    /// Represents a <see cref="DateTimeOffset"/> in the future, respect to the system time.
    /// </summary>
    public sealed class FutureTimestamp : AbstractDomainPrimitive<DateTimeOffset>, IComparable<FutureTimestamp>, IEquatable<FutureTimestamp>
    {
        /// <summary>
        /// Default error message.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' must be in the future respect to the system time.");

        private readonly string _asISOString;

        /// <summary>
        /// Validates input and builds new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue"></param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="rawValue"/> is not in the future.</exception>
        public FutureTimestamp(in DateTimeOffset rawValue) : this(rawValue, DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Validates input and builds new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Must be in the future</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <exception cref="ArgumentNullException">When <paramref name="errorMessage"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="rawValue"/> is not in the future.</exception>
        public FutureTimestamp(in DateTimeOffset rawValue, in Message errorMessage) : base(rawValue, errorMessage, (val, msg)
            => Validate(val, msg)) => _asISOString = Value.ToUniversalTime().ToString(Constants.ToISOStringFormat, Constants.ToISOStringFormatInfo);

        private static DateTimeOffset Validate(in DateTimeOffset rawValue, in Message errorMessage)
            => Arguments.GreaterThan(rawValue, DateTimeOffset.UtcNow, nameof(rawValue), errorMessage.Value);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(IDomainPrimitive{TRawType}?)"/>
        public int CompareTo(FutureTimestamp? other) => base.CompareTo(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(IDomainPrimitive{TRawType}?)"/>
        public override bool Equals(object? obj) => Equals(obj as FutureTimestamp);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)"/>
        public bool Equals(FutureTimestamp? other) => base.Equals(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()"/>
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc cref="PastOrPresentTimestamp.ToISOString"/>
        public string ToISOString() => _asISOString;

        #region Relational Operators

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in FutureTimestamp left, in FutureTimestamp right)
            => RelationalOperatorsOverloadHelper.NotEquals<FutureTimestamp>(left, right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in FutureTimestamp left, in FutureTimestamp right)
            => RelationalOperatorsOverloadHelper.Equals<FutureTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(in FutureTimestamp left, in FutureTimestamp right)
            => RelationalOperatorsOverloadHelper.LessThan<FutureTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(in FutureTimestamp left, in FutureTimestamp right)
            => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo<FutureTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(in FutureTimestamp left, in FutureTimestamp right)
            => RelationalOperatorsOverloadHelper.GreaterThan<FutureTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(in FutureTimestamp left, in FutureTimestamp right)
            => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo<FutureTimestamp>(left, right);
        
        #endregion //Relational Operators
    }
}