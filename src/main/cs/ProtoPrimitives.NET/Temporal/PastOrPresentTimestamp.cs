using Triplex.ProtoDomainPrimitives.Exceptions;
using System;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Temporal
{
    /// <summary>
    /// Represents a <see cref="DateTimeOffset"/> in the past or exactly as current time.
    /// </summary>
    public sealed class PastOrPresentTimestamp : AbstractDomainPrimitive<DateTimeOffset>
    {
        /// <summary>
        /// Default error message.
        /// </summary>
        public static readonly Message DefaultErrorMessage = new Message("'rawValue' must be current system time or some value in the past.");

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
        {
        }

        private static DateTimeOffset Validate(in DateTimeOffset rawValue, in Message errorMessage)
            => Arguments.LessThanOrEqualTo(rawValue, DateTimeOffset.UtcNow, nameof(rawValue), errorMessage.Value);
    }
}