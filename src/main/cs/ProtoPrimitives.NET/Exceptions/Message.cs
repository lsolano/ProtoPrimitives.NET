using System.Diagnostics.CodeAnalysis;

namespace Triplex.ProtoDomainPrimitives.Exceptions;

/// <summary>
/// Non-empty or null exception message. Comparison other <see cref="string"/>-related operation are done using 
/// <see cref="StringComparison.Ordinal"/>.
/// </summary>
public sealed class Message : IDomainPrimitive<string>, IComparable<Message>, IEquatable<Message>
{
    /// <summary>
    /// Validates input and returns new instance if everything is OK.
    /// </summary>
    /// <param name="rawValue">Can not be <see langword="null"/> or empty</param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="rawValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentFormatException">
    /// When <paramref name="rawValue"/> is empty or contains only white-spaces.
    /// </exception>
    public Message([NotNull] string rawValue)
        => Value = Arguments.NotEmptyOrWhiteSpaceOnly(rawValue, nameof(rawValue));

    /// <summary>
    /// Wrapped value.
    /// </summary>
    [NotNull]
    public string Value { get; }

    /// <summary>
    /// Compares with <see cref="StringComparison.Ordinal"/> strategy both wrapped values.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Message? other)
        => RelationalOperatorsOverloadHelper
            .SelfComparedToOther(this, other, o => string.Compare(Value, o.Value, StringComparison.Ordinal));

    /// <summary>
    /// Same as <see cref="Equals(Message?)"/> after casting the argument.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as Message);

    /// <summary>
    /// Same as wrapped value equality.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Message? other)
        => RelationalOperatorsOverloadHelper
            .SelfIsEqualsTo(this, other, o => Value.Equals(o.Value, StringComparison.Ordinal));

    /// <summary>
    /// Gets hashcode based on <see cref="Value"/>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Same as <see cref="Value"/>
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Value;


    #region Relational Operators

    /// <summary>
    /// Indicates if two instances are not equal.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Message left, Message right)
        => RelationalOperatorsOverloadHelper.NotEquals(left, right);

    /// <summary>
    /// Indicates if two instances are equals.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Message left, Message right)
        => RelationalOperatorsOverloadHelper.Equals(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(Message left, Message right)
        => RelationalOperatorsOverloadHelper.LessThan(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(Message left, Message right)
        => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(Message left, Message right)
        => RelationalOperatorsOverloadHelper.GreaterThan(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(Message left, Message right)
        => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo(left, right);

    #endregion //Relational Operators
}
