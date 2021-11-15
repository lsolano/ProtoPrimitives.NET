namespace Triplex.ProtoDomainPrimitives;

/// <summary>
/// <para>
/// Helper class to easy Relational Operators (==, !=, &lt;, &lt;=, &gt;, &gt;=) overload on types implementing 
/// <see cref="IComparable{T}"/> and <see cref="IEquatable{T}"/> where 'T' is the same type.
/// </para>
/// </summary>
/// <example>
/// For the simple class 
/// <code>
/// class Dog System.IComparable&lt;Dog&gt;, System.IEquatable&lt;Dog&gt; { /*...*/ }
/// </code>
/// If you want to overload all comparison operators just do:
/// <code>
/// class Dog System.IComparable&lt;Dog&gt;, System.IEquatable&lt;Dog&gt; {
///     /* regular class content including Equals and CompareTo implementations. */
/// 
///     #region Relational Operators Overload
/// 
///     public static bool operator !=(Dog left, Dog right)
///         =&gt; RelationalOperatorsOverloadHelper.NotEquals&lt;Dog&gt;(left, right);
/// 
///     public static bool operator ==(Dog left, Dog right)
///         =&gt; RelationalOperatorsOverloadHelper.Equals&lt;Dog&gt;(left, right);
/// 
///     public static bool operator &lt;(Dog left, Dog right)
///         =&gt; RelationalOperatorsOverloadHelper.LessThan&lt;Dog&gt;(left, right);
/// 
///     public static bool operator &lt;=(Dog left, Dog right) 
///         =&gt; RelationalOperatorsOverloadHelper.LessThanOrEqualsTo&lt;Dog&gt;(left, right);
/// 
///     public static bool operator &gt;(Dog left, Dog right) 
///         =&gt; RelationalOperatorsOverloadHelper.GreaterThan&lt;Dog&gt;(left, right);
/// 
///     public static bool operator &gt;=(Dog left, Dog right)
///         =&gt; RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo&lt;Dog&gt;(left, right);
/// 
///     #endregion //Relational Operators Overload
/// }
/// </code>
/// </example>
/// 
public static class RelationalOperatorsOverloadHelper
{
    /// <summary>
    /// Generic Comparison procedure. Before delegating to actual value comparator function, checks for 
    /// <see langword="null"/> and reference equality.
    /// </summary>
    /// <param name="self">
    /// Object receiving the <see cref="System.IComparable{TType}.CompareTo"/> message. 
    /// Can not be <see langword="null"/>.
    /// </param>
    /// <param name="other">
    /// Parameter of <see cref="System.IComparable{TType}.CompareTo"/>. Can be <see langword="null"/>.
    /// </param>
    /// <param name="valueComparator">
    /// Comparator function, receives <paramref name="other"/> after checking it for <see langword="null"/>.
    /// This function is assumed to be a lambda capturing 'this' and using it to compare based on 
    /// <typeparamref name="TType"/> comparison strategy.
    /// Can be <see langword="null"/>.
    /// </param>
    /// <typeparam name="TType"></typeparam>
    /// <returns></returns>
    public static int SelfComparedToOther<TType>(TType self, TType? other, Func<TType, int> valueComparator)
        where TType : class
    {
        Arguments.NotNull(self, nameof(self));
        Arguments.NotNull(valueComparator, nameof(valueComparator));

        if (other is null)
        {
            return 1;
        }
        else
        {
            return ReferenceEquals(self, other) ? 0 : valueComparator(other);
        }
    }

    /// <summary>
    /// Generic Equality procedure. Before delegating to actual value comparator function, checks for 
    /// <see langword="null"/> and reference equality.
    /// </summary>
    /// <param name="self">
    /// Object receiving the <see cref="object.Equals(object?)"/> message. Can not be <see langword="null"/>.
    /// </param>
    /// <param name="other">
    /// Parameter of <see cref="object.Equals(object?)"/>. Can be <see langword="null"/>.
    /// </param>
    /// <param name="valueComparator">
    /// Comparator function, receives <paramref name="other"/> after checking it for <see langword="null"/>.
    /// This function is assumed to be a lambda capturing 'this' and using it to compare based on 
    /// <typeparamref name="TType"/> equality strategy.
    /// Can be <see langword="null"/>.
    /// </param>
    /// <typeparam name="TType"></typeparam>
    /// <returns></returns>
    public static bool SelfIsEqualsTo<TType>(TType self, TType? other, Func<TType, bool> valueComparator)
        where TType : class
    {
        Arguments.NotNull(self, nameof(self));
        Arguments.NotNull(valueComparator, nameof(valueComparator));

        return other is not null && (ReferenceEquals(self, other) || valueComparator(other));
    }

    /// <summary>
    /// Not equals operator (!=) logic. Use it as:
    /// <code> 
    /// public static bool operator !=(TType left, TType right)
    ///     => RelationalOperatorsOverloadHelper.NotEquals&lt;TType&gt;(left, right);
    /// </code>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>The negation of <see cref="Equals{TType}(TType, TType)"/></returns>
    public static bool NotEquals<TType>(TType left, TType right)
        where TType : IComparable<TType>, IEquatable<TType>
        => !Equals(left, right);

    /// <summary>
    /// Equals operator (==) logic. Use it as
    /// <code> 
    /// public static bool operator ==(TType left, TType right)
    ///     => RelationalOperatorsOverloadHelper&lt;TType&gt;.Equals(left, right);
    /// </code>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if both are <see langword="null"/>, or result of <code>left.Equals(right)</code>
    /// </returns>
    public static bool Equals<TType>(TType left, TType right) where TType : IComparable<TType>, IEquatable<TType>
        => left is null ? right is null : left.Equals(right);

    /// <summary>
    /// Less than operator (&lt;) logic. Use it as
    /// <code> 
    /// public static bool operator &lt;(TType left, TType right)
    ///     => RelationalOperatorsOverloadHelper.LessThan&lt;TType&gt;(left, right);
    /// </code>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is <see langword="null"/> and <paramref name="right"/> is not, 
    /// or result of <code>left.CompareTo(right) &lt; 0</code></returns>
    public static bool LessThan<TType>(TType left, TType right)
        where TType : IComparable<TType>, IEquatable<TType>
        => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Less than or equals to operator (&lt;=) logic. Use it as
    /// <code> 
    /// public static bool operator &lt;=(TType left, TType right)
    ///     => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo&lt;TType&gt;(left, right);
    /// </code>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is <see langword="null"/>, 
    /// or result of <code>left.CompareTo(right) &lt;= 0</code></returns>
    public static bool LessThanOrEqualsTo<TType>(TType left, TType right)
        where TType : IComparable<TType>, IEquatable<TType>
        => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Greater than operator (&gt;) logic. Use it as
    /// <code> 
    /// public static bool operator &gt;(TType left, TType right)
    ///     => RelationalOperatorsOverloadHelper.GreaterThan&lt;TType&gt;(left, right);
    /// </code>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="false"/> if <paramref name="left"/> is <see langword="null"/>, 
    /// <see langword="true"/> if <paramref name="right"/> is <see langword="null"/>, 
    /// or result of <code>left.CompareTo(right) &gt; 0</code>
    /// </returns>
    public static bool GreaterThan<TType>(TType left, TType right)
        where TType : IComparable<TType>, IEquatable<TType>
    {
        if (left is null)
        {
            return false;
        }
        else if (right is null)
        {
            return true;
        }
        else
        {
            return left.CompareTo(right) > 0;
        }
    }

    /// <summary>
    /// Greater than or equals to operator (&gt;=) logic. Use it as
    /// <code> 
    /// public static bool operator &gt;=(TType left, TType right)
    ///     => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo&lt;TType&gt;(left, right);
    /// </code>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="right"/> is <see langword="null"/>, 
    /// <see langword="false"/> if <paramref name="left"/> is <see langword="null"/>, 
    /// or result of <code>left.CompareTo(right) &gt;= 0</code>
    /// </returns>
    public static bool GreaterThanOrEqualsTo<TType>(TType left, TType right)
        where TType : IComparable<TType>, IEquatable<TType>
    {
        if (right is null)
        {
            return true;

        }
        else if (left is null)
        {
            return false;
        }
        else
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
