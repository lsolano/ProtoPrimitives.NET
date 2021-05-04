using System;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Numerics
{
    /// <summary>
    /// Valid positive integer, meaning <code>&gt;= 1</code> (greater than or equals to one).
    /// </summary>
    public sealed class PositiveInteger : AbstractDomainPrimitive<int>, IEquatable<PositiveInteger>, IComparable<PositiveInteger>
    {
        /// <summary>
        /// Error message used when not provided.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' must be positive.");

        /// <summary>
        /// Error message used when <code>errorMessage</code> parameter is invalid.
        /// </summary>
        public static readonly Message InvalidCustomErrorMessageMessage = new Message("'errorMessage' could not be null, empty or white-space only.");

        /// <summary>
        /// Wraps the raw value and returns a new instance.
        /// </summary>
        /// <param name="rawValue">Must be positive</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="rawValue"/> is zero or negative.</exception>
        public PositiveInteger(in int rawValue) : this(rawValue, DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Wraps the raw value and returns a new instance.
        /// </summary>
        /// <param name="rawValue">Must be positive</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="rawValue"/> is zero or negative.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="errorMessage"/> is <see langword="null"/>.</exception>
        public PositiveInteger(in int rawValue, in Message errorMessage) : base(rawValue, errorMessage, (val, msg) => Validate(val, msg))
        {
        }

        private static int Validate(in int rawValue, in Message errorMessage)
            => Arguments.GreaterThan(rawValue, 0, nameof(rawValue), errorMessage.Value);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(IDomainPrimitive{TRawType}?)" />
        public int CompareTo(PositiveInteger? other) => base.CompareTo(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
        public override bool Equals(object? obj) => Equals(obj as PositiveInteger);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(IDomainPrimitive{TRawType}?)" />
        public bool Equals(PositiveInteger? other) => base.Equals(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
        public override int GetHashCode() => base.GetHashCode();


        #region Relational Operators

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in PositiveInteger left, in PositiveInteger right)
            => RelationalOperatorsOverloadHelper.NotEquals<PositiveInteger>(left, right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in PositiveInteger left, in PositiveInteger right)
            => RelationalOperatorsOverloadHelper.Equals<PositiveInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(in PositiveInteger left, in PositiveInteger right)
            => RelationalOperatorsOverloadHelper.LessThan<PositiveInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(in PositiveInteger left, in PositiveInteger right)
            => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo<PositiveInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(in PositiveInteger left, in PositiveInteger right)
            => RelationalOperatorsOverloadHelper.GreaterThan<PositiveInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(in PositiveInteger left, in PositiveInteger right)
            => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo<PositiveInteger>(left, right);
        
        #endregion //Relational Operators
    }
}
