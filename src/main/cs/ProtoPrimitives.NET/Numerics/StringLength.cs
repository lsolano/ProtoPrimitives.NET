using ProtoPrimitives.NET.Exceptions;
using Triplex.Validations;

namespace ProtoPrimitives.NET.Numerics
{
    /// <summary>
    /// Represents a valid string length, meaning from zero (0) to <see cref="System.Int32.MaxValue"/>
    /// </summary>
    public sealed class StringLength : AbstractDomainPrimitive<int>
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
        /// Initializes an intance after validating provided input.
        /// </summary>
        /// <param name="rawValue"></param>
        public StringLength(int rawValue) : this(rawValue, DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Initializes an intance after validating provided input.
        /// </summary>
        /// <param name="rawValue"></param>
        /// <param name="errorMessage">Can not be <see langword="null"/></param>
        public StringLength(int rawValue, Message errorMessage) : base(rawValue, errorMessage, Validate)
        {
        }

        private static int Validate(int rawValue, Message errorMessage) => Arguments.GreaterThanOrEqualTo(rawValue, 0, nameof(rawValue));
    }
}
