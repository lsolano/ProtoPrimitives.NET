using System;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Numerics
{
    /// <summary>
    /// Valid negative integer, meaning <code>&lt; 0</code> (less than zero).
    /// </summary>
    public sealed class NegativeInteger : AbstractDomainPrimitive<int>, IEquatable<NegativeInteger>, IComparable<NegativeInteger>
    {
        /// <summary>
        /// Error message used when not provided.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' must be negative.");

        /// <summary>
        /// Error message used when <code>errorMessage</code> parameter is invalid.
        /// </summary>
        public static readonly Message InvalidCustomErrorMessageMessage = new Message("'errorMessage' could not be null, empty or white-space only.");

        /// <summary>
        /// Wraps the raw value and returns a new instance.
        /// </summary>
        /// <param name="rawValue">Must be negative</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="rawValue"/> is zero or positive.</exception>
        public NegativeInteger(in int rawValue) : this(rawValue, DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Wraps the raw value and returns a new instance.
        /// </summary>
        /// <param name="rawValue">Must be positive</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="rawValue"/> is zero or positive.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="errorMessage"/> is <see langword="null"/>.</exception>
        public NegativeInteger(in int rawValue, in Message errorMessage) : base(rawValue, errorMessage, (val, msg) => Validate(val, msg))
        {
        }

        private static int Validate(in int rawValue, in Message errorMessage)
            => Arguments.LessThan(rawValue, 0, nameof(rawValue), errorMessage.Value);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(IDomainPrimitive{TRawType}?)" />
        public int CompareTo(NegativeInteger? other) => base.CompareTo(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
        public override bool Equals(object? obj) => Equals(obj as NegativeInteger);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(IDomainPrimitive{TRawType}?)" />
        public bool Equals(NegativeInteger? other) => base.Equals(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
        public override int GetHashCode() => base.GetHashCode();


        #region Relational Operators

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in NegativeInteger left, in NegativeInteger right)
            => RelationalOperatorsOverloadHelper.NotEquals<NegativeInteger>(left, right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in NegativeInteger left, in NegativeInteger right)
            => RelationalOperatorsOverloadHelper.Equals<NegativeInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(in NegativeInteger left, in NegativeInteger right)
            => RelationalOperatorsOverloadHelper.LessThan<NegativeInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(in NegativeInteger left, in NegativeInteger right)
            => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo<NegativeInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(in NegativeInteger left, in NegativeInteger right)
            => RelationalOperatorsOverloadHelper.GreaterThan<NegativeInteger>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(in NegativeInteger left, in NegativeInteger right)
            => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo<NegativeInteger>(left, right);
        
        #endregion //Relational Operators
    }
}
