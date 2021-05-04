using System;

namespace Triplex.ProtoDomainPrimitives
{
    /// <summary>
    /// <para>
    /// Helper class to easy Relational Operators (==, !=, &lt;, &lt;=, &gt;, &gt;=) overload on types implementing 
    /// <see cref="System.IComparable{T}"/> and <see cref="System.IEquatable{T}"/> where 'T' is the same type.
    /// </para>
    /// </summary>
    /// <example>
    ///     /// For the simple class 
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
    ///     public static bool operator !=(in Dog left, in Dog right)
    ///         =&gt; RelationalOperatorsOverloadHelper.NotEquals&lt;Dog&gt;(left, right);
    /// 
    ///     public static bool operator ==(in Dog left, in Dog right)
    ///         =&gt; RelationalOperatorsOverloadHelper.Equals&lt;Dog&gt;(left, right);
    /// 
    ///     public static bool operator &lt;(in Dog left, in Dog right)
    ///         =&gt; RelationalOperatorsOverloadHelper.LessThan&lt;Dog&gt;(left, right);
    /// 
    ///     public static bool operator &lt;=(in Dog left, in Dog right) 
    ///         =&gt; RelationalOperatorsOverloadHelper.LessThanOrEqualsTo&lt;Dog&gt;(left, right);
    /// 
    ///     public static bool operator &gt;(in Dog left, in Dog right) 
    ///         =&gt; RelationalOperatorsOverloadHelper.GreaterThan&lt;Dog&gt;(left, right);
    /// 
    ///     public static bool operator &gt;=(in Dog left, in Dog right)
    ///         =&gt; RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo&lt;Dog&gt;(left, right);
    /// 
    ///     #endregion //Relational Operators Overload
    /// }
    /// </code>
    /// </example>
    /// 
    //public static class RelationalOperatorsOverloadHelper<TType> where TType : IComparable<TType>, IEquatable<TType> {
    public static class RelationalOperatorsOverloadHelper {

        /// <summary>
        /// Not equals operator (!=) logic. Use it as:
        /// <code> 
        /// public static bool operator !=(in TType left, in TType right)
        ///     => RelationalOperatorsOverloadHelper.NotEquals&lt;TType&gt;(left, right);
        /// </code>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The negation of <see cref="RelationalOperatorsOverloadHelper.Equals{TType}(in TType, in TType)"/></returns>
        public static bool NotEquals<TType>(in TType left, in TType right) where TType : IComparable<TType>, IEquatable<TType>
            => !RelationalOperatorsOverloadHelper.Equals(left, right);

        /// <summary>
        /// Equals operator (==) logic. Use it as
        /// <code> 
        /// public static bool operator ==(in TType left, in TType right)
        ///     => RelationalOperatorsOverloadHelper&lt;TType&gt;.Equals(left, right);
        /// </code>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns><see langword="true"/> if both are <see langword="null"/>, or result of <code>left.Equals(right)</code></returns>
        public static bool Equals<TType>(in TType left, in TType right) where TType : IComparable<TType>, IEquatable<TType>
        {
            if (left is null) {
                return right is null;
            }

            return left.Equals(right);
        }
        
        /// <summary>
        /// Less than operator (&lt;) logic. Use it as
        /// <code> 
        /// public static bool operator &lt;(in TType left, in TType right)
        ///     => RelationalOperatorsOverloadHelper.LessThan&lt;TType&gt;(left, right);
        /// </code>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is <see langword="null"/> and <paramref name="right"/> is not, 
        /// or result of <code>left.CompareTo(right) &lt; 0</code></returns>
        public static bool LessThan<TType>(in TType left, in TType right)  where TType : IComparable<TType>, IEquatable<TType>
        {
            if (left is null) {
                return right is not null;
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Less than or equals to operator (&lt;=) logic. Use it as
        /// <code> 
        /// public static bool operator &lt;=(in TType left, in TType right)
        ///     => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo&lt;TType&gt;(left, right);
        /// </code>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is <see langword="null"/>, 
        /// or result of <code>left.CompareTo(right) &lt;= 0</code></returns>
        public static bool LessThanOrEqualsTo<TType>(in TType left, in TType right)  where TType : IComparable<TType>, IEquatable<TType>
        {
            if (left is null) {
                return true;
            }

            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Greater than operator (&gt;) logic. Use it as
        /// <code> 
        /// public static bool operator &gt;(in TType left, in TType right)
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
        public static bool GreaterThan<TType>(in TType left, in TType right) where TType : IComparable<TType>, IEquatable<TType>
        {
            if (left is null) {
                return false;
            
            } else if (right is null) {
                return true;
            }

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Greater than or equals to operator (&gt;=) logic. Use it as
        /// <code> 
        /// public static bool operator &gt;=(in TType left, in TType right)
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
        public static bool GreaterThanOrEqualsTo<TType>(in TType left, in TType right) where TType : IComparable<TType>, IEquatable<TType>
        {
            if (right is null) {
                return true;

            } else if (left is null) {
                return false;
            }

            return left.CompareTo(right) >= 0;
        }
    }
}