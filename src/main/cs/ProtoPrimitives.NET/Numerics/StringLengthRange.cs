using System;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Numerics
{
    /// <summary>
    /// Defines a length's range for strings.
    /// </summary>
    public sealed class StringLengthRange
    {
        /// <summary>
        /// Validates that the given input conform a valid range and sets properties.
        /// </summary>
        /// <param name="min">Can be equals to <paramref name="max"/> but not <see langword="nulll"/></param>
        /// <param name="max">Can be equals to <paramref name="min"/> but not <see langword="nulll"/></param>
        public StringLengthRange(in StringLength min, in StringLength max)
        {
            Validate(min, max);

            (Min, Max) = (min, max);
        }

        /// <summary>
        /// Min length (inclusive)
        /// </summary>
        public StringLength Min { get; }

        /// <summary>
        /// Max length (inclusive)
        /// </summary>
        public StringLength Max { get; }

        internal static void Validate(in StringLength min, in StringLength max)
        {
            Arguments.NotNull(min, nameof(min));
            Arguments.NotNull(max, nameof(max));

            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, $"{nameof(min)} must be less than or equals to (<=) {nameof(max)} ({max}).");
            }
        }
    }
}
