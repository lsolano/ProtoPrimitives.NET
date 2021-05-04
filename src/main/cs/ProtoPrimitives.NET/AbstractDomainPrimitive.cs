using System;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives
{
    /// <summary>
    /// Useful base class when the domain primitive will be a proxy of the wrapped value. All operations, except input validation, are based on the wrapped type.
    /// </summary>
    /// <typeparam name="TRawType">Wrapped type</typeparam>
    public abstract class AbstractDomainPrimitive<TRawType> : IDomainPrimitive<TRawType> where TRawType : IComparable<TRawType>, IEquatable<TRawType>
    {
        /// <summary>
        /// Builds a new instance calling <paramref name="validator"/> first.
        /// </summary>
        /// <param name="rawValue">Value to wrap.</param>
        /// <param name="errorMessage">Value to wrap.</param>
        /// <param name="validator">Validator function, must perform all validations and throw exceptions, if everything is OK returns the <paramref name="rawValue"/></param>
        /// <exception cref="ArgumentNullException">When <paramref name="validator"/> is <see langword="null"/>.</exception>
        #pragma warning disable CS8618 // Non-null property must have a value (Value)
        protected AbstractDomainPrimitive(in TRawType? rawValue, in Message errorMessage, in Func<TRawType, Message, TRawType> validator)  {
            if (rawValue is null) {
                throw new ArgumentNullException(nameof(rawValue));
            }
            Arguments.NotNull(validator, nameof(validator));
            Arguments.NotNull(errorMessage, nameof(errorMessage));

            Value = validator(rawValue, errorMessage);

            State.IsTrue(Value is not null, "Fatal error, value can not be null here.");
        }
        #pragma warning restore CS8618 // Non-null property must have a value (Value)

        /// <summary>
        /// Wrapped value.
        /// </summary>
        public TRawType Value { get; }

        /// <summary>
        /// Calls <see cref="IEquatable{TRawType}.Equals(TRawType)"/> casting <paramref name="obj"/> before.
        /// </summary>
        /// <param name="obj">Comparison target.</param>
        /// <returns></returns>
        public override bool Equals(object? obj) => Equals(obj as AbstractDomainPrimitive<TRawType>);

        /// <summary>
        /// Same as <see cref="Value"/>'s hash-code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Same as <see cref="Value"/>'s ToString.
        /// </summary>
        /// <returns></returns>
        #pragma warning disable CS8603 // Value can not be null here.
        public override string ToString() => Value.ToString();
        #pragma warning restore CS8603 // Value can not be null here.

        /// <summary>
        /// Same as wrapped instance CompareTo.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(IDomainPrimitive<TRawType>? other)
        {
            if (other is null)
            {
                return 1;
            }

            return ReferenceEquals(this, other) ? 0 : Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Same as wrapped instances equals.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(IDomainPrimitive<TRawType>? other)
        {
            if (other is null)
            {
                return false;
            }

            return ReferenceEquals(this, other) || Value.Equals(other.Value);
        }

        /// <summary>
        /// Indicates if two instances are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(in AbstractDomainPrimitive<TRawType> left, in AbstractDomainPrimitive<TRawType> right) => !(left == right);

        /// <summary>
        /// Indicates if two instances are equals.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(in AbstractDomainPrimitive<TRawType> left, in AbstractDomainPrimitive<TRawType> right)
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
        public static bool operator <(in AbstractDomainPrimitive<TRawType> left, in AbstractDomainPrimitive<TRawType> right) {
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
        public static bool operator <=(in AbstractDomainPrimitive<TRawType> left, in AbstractDomainPrimitive<TRawType> right) {
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
        public static bool operator >(in AbstractDomainPrimitive<TRawType> left, in AbstractDomainPrimitive<TRawType> right) {
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
        public static bool operator >=(in AbstractDomainPrimitive<TRawType> left, in AbstractDomainPrimitive<TRawType> right) {
            if (right is null) {
                return true;

            } else if (left is null) {
                return false;
            }

            return left.CompareTo(right) >= 0;
        }
    }
}
