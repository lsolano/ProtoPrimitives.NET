using System;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Numerics
{
    /// <summary>
    /// Represents a valid string length, meaning from zero (0) to <see cref="System.Int32.MaxValue"/>
    /// </summary>
    public sealed class StringLength : AbstractDomainPrimitive<int>, IEquatable<StringLength>, IComparable<StringLength>
    {
        /// <summary>
        /// Default error message if the provided value is not valid.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' must be zero or positive.");

        /// <summary>
        /// Minimum allowed.
        /// </summary>
        public static readonly  StringLength Min = new StringLength(0);

        /// <summary>
        /// Maximum allowed.
        /// </summary>
        public static readonly  StringLength Max = new StringLength(int.MaxValue);

        /// <summary>
        /// Initializes an instance after validating provided input.
        /// </summary>
        /// <param name="rawValue"></param>
        public StringLength(in int rawValue) : this(rawValue, DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Initializes an instance after validating provided input.
        /// </summary>
        /// <param name="rawValue"></param>
        /// <param name="errorMessage">Can not be <see langword="null"/></param>
        public StringLength(in int rawValue, in Message errorMessage) : base(rawValue, errorMessage, (val, msg) => Validate(val, msg))
        {
        }

        private static int Validate(in int rawValue, in Message errorMessage)
            => Arguments.GreaterThanOrEqualTo(rawValue, 0, nameof(rawValue), Arguments.NotNull(errorMessage, nameof(errorMessage)).Value);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(IDomainPrimitive{TRawType}?)" />
        public int CompareTo(StringLength? other) => base.CompareTo(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)" />
        public override bool Equals(object? obj) => Equals(obj as StringLength);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(IDomainPrimitive{TRawType}?)" />
        public bool Equals(StringLength? other) => base.Equals(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()" />
        public override int GetHashCode() => base.GetHashCode();


        #region Relational Operators

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in StringLength left, in StringLength right)
            => RelationalOperatorsOverloadHelper.NotEquals<StringLength>(left, right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in StringLength left, in StringLength right)
            => RelationalOperatorsOverloadHelper.Equals<StringLength>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(in StringLength left, in StringLength right)
            => RelationalOperatorsOverloadHelper.LessThan<StringLength>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(in StringLength left, in StringLength right)
            => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo<StringLength>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(in StringLength left, in StringLength right)
            => RelationalOperatorsOverloadHelper.GreaterThan<StringLength>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(in StringLength left, in StringLength right)
            => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo<StringLength>(left, right);
        
        #endregion //Relational Operators
    }
}
