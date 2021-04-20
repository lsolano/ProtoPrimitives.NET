using System;

namespace Triplex.ProtoDomainPrimitives
{
    /// <summary>
    /// Represent a domain primitive wrapping some raw type, usually language primitive types such as <see cref="string"/> and <see cref="int"/>.
    /// </summary>
    public interface IDomainPrimitive<TRawType> : IComparable<IDomainPrimitive<TRawType>>, IEquatable<IDomainPrimitive<TRawType>>
    {
        /// <summary>
        /// Wrapped value.
        /// </summary>
        TRawType Value { get; }
    }
}
