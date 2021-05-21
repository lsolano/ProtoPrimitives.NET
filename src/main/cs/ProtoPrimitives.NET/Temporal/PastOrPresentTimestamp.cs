using System;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Temporal
{
    /// <summary>
    /// Represents a <see cref="DateTimeOffset"/> in the past or exactly as current time.
    /// </summary>
    public sealed class PastOrPresentTimestamp : AbstractDomainPrimitive<DateTimeOffset>, IComparable<PastOrPresentTimestamp>, IEquatable<PastOrPresentTimestamp>
    {
        /// <summary>
        /// Default error message.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' must be current system time or some value in the past.");
        
        private readonly string _asISOString;

        /// <summary>
        /// Validates input and builds new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Must be present or past time</param>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="rawValue"/> is in the future.</exception>
        public PastOrPresentTimestamp(in DateTimeOffset rawValue) : this(rawValue, DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Validates input and builds new instance if everything is OK.
        /// </summary>
        /// <param name="rawValue">Must be present or past time</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <exception cref="ArgumentNullException">When <paramref name="errorMessage"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="rawValue"/> is in the future.</exception>
        public PastOrPresentTimestamp(in DateTimeOffset rawValue, in Message errorMessage) : base(rawValue, errorMessage, (val, msg) => Validate(val, msg))
            => _asISOString = Value.ToUniversalTime().ToString(Constants.ToISOStringFormat, Constants.ToISOStringFormatInfo);

        private static DateTimeOffset Validate(in DateTimeOffset rawValue, in Message errorMessage)
            => Arguments.LessThanOrEqualTo(rawValue, DateTimeOffset.UtcNow, nameof(rawValue), errorMessage.Value);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.CompareTo(IDomainPrimitive{TRawType}?)"/>
        public int CompareTo(PastOrPresentTimestamp? other) => base.CompareTo(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(object?)"/>
        public override bool Equals(object? obj) => Equals(obj as PastOrPresentTimestamp);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.Equals(IDomainPrimitive{TRawType}?)"/>
        public bool Equals(PastOrPresentTimestamp? other) => base.Equals(other);

        /// <inheritdoc cref="AbstractDomainPrimitive{TRawType}.GetHashCode()"/>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// <para>
        /// A string in simplified extended ISO format (<see href="https://en.wikipedia.org/wiki/ISO_8601">ISO 8601</see>) as UTC. 
        /// The year is formated using four (4) positions. Month, day, hour, minute, and second are formated using two (2) positions. 
        /// The hour uses 24H format (00-23). The milliseconds part uses three (3) positions.
        /// </para>
        /// <para>
        /// Inspired on <see href="https://tc39.es/ecma262/#sec-date.prototype.toisostring">21.4.4.36 Date.prototype.toISOString()</see> 
        /// as of '2021-05-21T16:29:04.613Z' (formated accordinly to this method).
        /// </para>
        /// </summary>
        /// <example>
        /// 2021-05-21T15:23:53.162Z
        /// </example>
        /// <returns></returns>
        public string ToISOString() => _asISOString;


        #region Relational Operators

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in PastOrPresentTimestamp left, in PastOrPresentTimestamp right)
            => RelationalOperatorsOverloadHelper.NotEquals<PastOrPresentTimestamp>(left, right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in PastOrPresentTimestamp left, in PastOrPresentTimestamp right)
            => RelationalOperatorsOverloadHelper.Equals<PastOrPresentTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(in PastOrPresentTimestamp left, in PastOrPresentTimestamp right)
            => RelationalOperatorsOverloadHelper.LessThan<PastOrPresentTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(in PastOrPresentTimestamp left, in PastOrPresentTimestamp right)
            => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo<PastOrPresentTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(in PastOrPresentTimestamp left, in PastOrPresentTimestamp right)
            => RelationalOperatorsOverloadHelper.GreaterThan<PastOrPresentTimestamp>(left, right);

        /// <summary>
        /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(in PastOrPresentTimestamp left, in PastOrPresentTimestamp right)
            => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo<PastOrPresentTimestamp>(left, right);
        
        #endregion //Relational Operators
    }
}