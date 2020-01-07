using ProtoPrimitives.NET.Exceptions;
using System;
using Triplex.Validations;

namespace ProtoPrimitives.NET.Numerics
{
    /// <summary>
    /// Valid negative integer, meaning <code>&lt; 0</code> (less than zero).
    /// </summary>
    public sealed class NegativeInteger : AbstractDomainPrimitive<int>
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

        private static int Validate(in int rawValue, in Message errorMessage) => Arguments.LessThan(rawValue, 0, nameof(rawValue), errorMessage.Value);
    }
}
