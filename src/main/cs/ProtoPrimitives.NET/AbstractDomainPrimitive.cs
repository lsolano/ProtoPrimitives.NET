using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives;

/// <summary>
/// Useful base class when the domain primitive will be a proxy of the wrapped value. 
/// All operations, except input validation, are based on the wrapped type.
/// </summary>
/// <typeparam name="TRawType">Wrapped type</typeparam>
public class AbstractDomainPrimitive<TRawType> :
    IDomainPrimitive<TRawType>,
    IComparable<AbstractDomainPrimitive<TRawType>>,
    IEquatable<AbstractDomainPrimitive<TRawType>>,
    IEqualityComparer<AbstractDomainPrimitive<TRawType>>
    where TRawType : IComparable<TRawType>, IEquatable<TRawType>
{
    /// <summary>
    /// Hashcode used when instances are null.
    /// </summary>
    private const int DefaultHashCode = 31;

    /// <summary>
    /// Builds a new instance calling <paramref name="validator"/> first.
    /// </summary>
    /// <param name="rawValue">Value to wrap.</param>
    /// <param name="errorMessage">Value to wrap.</param>
    /// <param name="validator">Validator function, must perform all validations and throw exceptions, 
    /// if everything is OK returns the <paramref name="rawValue"/>
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="validator"/> is <see langword="null"/>.
    /// </exception>
#pragma warning disable CS8618 // Non-null property must have a value (Value)
    protected AbstractDomainPrimitive([NotNull] TRawType? rawValue, [NotNull] Message errorMessage,
        [NotNull] Func<TRawType, Message, TRawType> validator)
    {
        if (rawValue is null)
        {
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
    [NotNull]
    public TRawType Value { get; }

    /// <summary>
    /// Calls <see cref="IEquatable{TRawType}.Equals(TRawType)"/> casting <paramref name="obj"/> before.
    /// </summary>
    /// <param name="obj">Comparison target.</param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as AbstractDomainPrimitive<TRawType>);

    /// <summary>
    /// Same as wrapped instances <see cref="IEquatable{T}.Equals"/>>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public virtual bool Equals(AbstractDomainPrimitive<TRawType>? other)
        => RelationalOperatorsOverloadHelper.SelfIsEqualsTo(this, other, o => Value.Equals(o.Value));

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
    /// Same as wrapped instance <see cref="IComparable{T}.CompareTo"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(AbstractDomainPrimitive<TRawType>? other)
        => RelationalOperatorsOverloadHelper
            .SelfComparedToOther(this, other, o => Value.CompareTo(o.Value));

    /// <summary>
    /// Compares two domain primitives.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool Equals(AbstractDomainPrimitive<TRawType>? x, AbstractDomainPrimitive<TRawType>? y)
        => x is not null ?
            RelationalOperatorsOverloadHelper.SelfIsEqualsTo(x, y, notNullY => x.Equals(y)) :
            y is null;

    /// <summary>
    /// Categorization function.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int GetHashCode([DisallowNull] AbstractDomainPrimitive<TRawType> obj)
        => obj?.GetHashCode() ?? DefaultHashCode;

    #region Relational Operators

    /// <summary>
    /// Indicates if two instances are not equal.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(AbstractDomainPrimitive<TRawType> left, AbstractDomainPrimitive<TRawType> right)
        => RelationalOperatorsOverloadHelper.NotEquals(left, right);

    /// <summary>
    /// Indicates if two instances are equals.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(AbstractDomainPrimitive<TRawType> left, AbstractDomainPrimitive<TRawType> right)
        => RelationalOperatorsOverloadHelper.Equals(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(AbstractDomainPrimitive<TRawType> left, AbstractDomainPrimitive<TRawType> right)
        => RelationalOperatorsOverloadHelper.LessThan(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(AbstractDomainPrimitive<TRawType> left, AbstractDomainPrimitive<TRawType> right)
        => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(AbstractDomainPrimitive<TRawType> left, AbstractDomainPrimitive<TRawType> right)
        => RelationalOperatorsOverloadHelper.GreaterThan(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(AbstractDomainPrimitive<TRawType> left, AbstractDomainPrimitive<TRawType> right)
        => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo(left, right);

    #endregion //Relational Operators
}
