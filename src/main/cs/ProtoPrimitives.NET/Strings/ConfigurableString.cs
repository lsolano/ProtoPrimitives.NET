using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;

namespace Triplex.ProtoDomainPrimitives.Strings;

/// <summary>
/// <para>
/// String wrapper with several format and structural validations upon construction.
/// Use <see cref="ConfigurableString.Builder"/> to create instances of this class.
/// </para>
/// <para>
/// Be aware that wrapped value van not be <see langword="null"/> above all other validations.
/// </para>
/// </summary>
public sealed class ConfigurableString : IDomainPrimitive<string>, IComparable<ConfigurableString>, IEquatable<ConfigurableString>
{
    private readonly StringComparison _comparisonStrategy;

    private ConfigurableString(string rawValue, StringComparison comparisonStrategy)
    {
        Value = rawValue;
        _comparisonStrategy = comparisonStrategy;
    }

    /// <summary>
    /// Actual value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Checks for equality using the strategy specified by builder <see cref="Builder.WithComparisonStrategy"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as ConfigurableString);

    /// <summary>
    /// Checks for equality using the strategy specified by builder <see cref="Builder.WithComparisonStrategy"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IDomainPrimitive<string>? other)
        => RelationalOperatorsOverloadHelper.SelfIsEqualsTo(this, other,
            o => Value.Equals(o.Value, _comparisonStrategy));

    /// <summary>
    /// Categorize based on values and provided comparison strategy.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Value.GetHashCode(_comparisonStrategy);

    /// <inheritdoc cref="object.ToString()"/>
    public override string ToString() => Value;

    /// <summary>
    /// Compares using the strategy specified by builder <see cref="Builder.WithComparisonStrategy"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(ConfigurableString? other)
        => RelationalOperatorsOverloadHelper
            .SelfComparedToOther(this, other, o => string.Compare(Value, o.Value, _comparisonStrategy));


    /// <summary>
    /// Checks for equality using the strategy specified by builder <see cref="Builder.WithComparisonStrategy"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ConfigurableString? other)
        => RelationalOperatorsOverloadHelper
            .SelfIsEqualsTo<ConfigurableString>(this, other, o => Value.Equals(o.Value, _comparisonStrategy));

    /// <summary>
    /// Indicates if two instances are not equal.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(ConfigurableString left, ConfigurableString right)
        => RelationalOperatorsOverloadHelper.NotEquals<ConfigurableString>(left, right);

    /// <summary>
    /// Indicates if two instances are equals.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(ConfigurableString left, ConfigurableString right)
        => RelationalOperatorsOverloadHelper.Equals<ConfigurableString>(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is less than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(ConfigurableString left, ConfigurableString right)
        => RelationalOperatorsOverloadHelper.LessThan<ConfigurableString>(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is less than or equals to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(ConfigurableString left, ConfigurableString right)
        => RelationalOperatorsOverloadHelper.LessThanOrEqualsTo<ConfigurableString>(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is greater than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(ConfigurableString left, ConfigurableString right)
        => RelationalOperatorsOverloadHelper.GreaterThan<ConfigurableString>(left, right);

    /// <summary>
    /// Indicates if <paramref name="left"/> is greater than or equals to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(ConfigurableString left, ConfigurableString right)
        => RelationalOperatorsOverloadHelper.GreaterThanOrEqualsTo<ConfigurableString>(left, right);

    /// <summary>
    /// Fluent builder for <see cref="ConfigurableString"/>s.
    /// This builder is for only one usage, after calling <see cref="Build(string)"/> or 
    /// <see cref="Build(string, Action{string})"/>
    /// subsequent calls will throw <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class Builder
    {
        private static readonly Action<string> NoOpCustomParser = (val) => { };
        private static readonly Message DefaultTooShortErrorMessage = new("Input string is too short.");
        private static readonly Message DefaultTooLongErrorMessage = new("Input string is too long.");
        private static readonly Message DefaultInvalidCharactersErrorMessage =
            new("Input string contains invalid characters.");
        private static readonly Message DefaultInvalidFormatErrorMessage =
            new("Input string has an invalid format.");

        private bool _built;

        private StringComparison _comparisonStrategy = StringComparison.Ordinal;
        private readonly Message _argumentNullErrorMessage;
        private Message _tooShortErrorMessage = DefaultTooShortErrorMessage;
        private Message _tooLongErrorMessage = DefaultTooLongErrorMessage;
        private Message _invalidCharactersErrorMessage = DefaultInvalidCharactersErrorMessage;
        private Message _invalidFormatErrorMessage = DefaultInvalidFormatErrorMessage;

        private StringLength? _minLength;
        private StringLength? _maxLength;
        private bool _requiresTrimmed;
        private bool _allowLeadingWhiteSpace = true;
        private bool _allowTrailingWhiteSpace = true;
        private bool _allowWhiteSpacesOnly = true;
        private Regex? _invalidCharsRegex;
        private Regex? _validFormatRegex;

        private bool DoesNotAllowLeadingWhiteSpace => !_allowLeadingWhiteSpace;
        private bool DoesNotAllowTrailingWhiteSpace => !_allowTrailingWhiteSpace;
        private bool DoesNotAllowWhiteSpacesOnly => !_allowWhiteSpacesOnly;

        /// <summary>
        /// Creates a builder with the error message to be used when input value is <see langword="null"/>.
        /// </summary>
        /// <param name="argumentNullErrorMessage">Required</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="argumentNullErrorMessage"/> is <see langword="null"/>
        /// </exception>
        public Builder(Message argumentNullErrorMessage) : this(argumentNullErrorMessage, false)
        {
        }

        /// <summary>
        /// Creates a builder with the error message to be used when input value is <see langword="null"/>.
        /// </summary>
        /// <param name="argumentNullErrorMessage">Required</param>
        /// <param name="useSingleMessage">
        /// If <see langword="true"/> the same error message (<paramref name="argumentNullErrorMessage"/>) 
        /// will be used on all exception situations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="argumentNullErrorMessage"/> is <see langword="null"/>
        /// </exception>
        public Builder(Message argumentNullErrorMessage, bool useSingleMessage)
        {
            _argumentNullErrorMessage = Arguments.NotNull(argumentNullErrorMessage,
                nameof(argumentNullErrorMessage));

            if (useSingleMessage)
            {
                _tooShortErrorMessage = _argumentNullErrorMessage;
                _tooLongErrorMessage = _argumentNullErrorMessage;
                _invalidCharactersErrorMessage = _argumentNullErrorMessage;
                _invalidFormatErrorMessage = _argumentNullErrorMessage;
            }
        }

        /// <summary>
        /// If not set, defaults to <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="comparisonStrategy"></param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When <paramref name="comparisonStrategy"/> is not a valid enumeration member.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If already built.
        /// </exception>
        public Builder WithComparisonStrategy(StringComparison comparisonStrategy)
            => CheckPreconditionsAndExecute(() => _comparisonStrategy =
                Arguments.ValidEnumerationMember(comparisonStrategy, nameof(comparisonStrategy)));

        /// <summary>
        /// Sets the minimum allowed length.
        /// </summary>
        /// <param name="minLength">Can not be <see langword="null"/>.</param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithMinLength(StringLength minLength)
            => CheckPreconditionsAndExecute(() => _minLength = Arguments.NotNull(minLength, nameof(minLength)));

        /// <summary>
        /// Sets the minimum allowed length and associated violation message.
        /// </summary>
        /// <param name="minLength">Can not be <see langword="null"/>.</param>
        /// <param name="tooShortErrorMessage">Can not be <see langword="null"/>.</param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithMinLength(StringLength minLength, Message tooShortErrorMessage)
        {
            return CheckPreconditionsAndExecute(() =>
            {
                _tooShortErrorMessage = Arguments.NotNull(tooShortErrorMessage, nameof(tooShortErrorMessage));
                _minLength = Arguments.NotNull(minLength, nameof(minLength));
            });
        }

        /// <summary>
        /// Sets the maximum allowed length and associated violation message.
        /// </summary>
        /// <param name="maxLength">Can not be <see langword="null"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithMaxLength(StringLength maxLength)
            => CheckPreconditionsAndExecute(() => _maxLength = Arguments.NotNull(maxLength, nameof(maxLength)));

        /// <summary>
        /// Sets the maximum allowed length and associated violation message.
        /// </summary>
        /// <param name="maxLength">Can not be <see langword="null"/>.</param>
        /// <param name="tooLongErrorMessage">Can not be <see langword="null"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithMaxLength(StringLength maxLength, Message tooLongErrorMessage)
        {
            return CheckPreconditionsAndExecute(() =>
            {
                _tooLongErrorMessage = Arguments.NotNull(tooLongErrorMessage, nameof(tooLongErrorMessage));
                _maxLength = Arguments.NotNull(maxLength, nameof(maxLength));
            });
        }

        /// <summary>
        /// Sets both minimum length and maximum length with associated error messages.
        /// </summary>
        /// <param name="lengthRange">Can not be <see langword="null"/>.</param>
        /// <param name="tooShortErrorMessage">Can not be <see langword="null"/>.</param>
        /// <param name="tooLongErrorMessage">Can not be <see langword="null"/>.</param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithLengthRange(StringLengthRange lengthRange, Message tooShortErrorMessage, Message
            tooLongErrorMessage)
        {
            return CheckPreconditionsAndExecute(() =>
            {
                Arguments.NotNull(lengthRange, nameof(lengthRange));

                _tooShortErrorMessage = Arguments.NotNull(tooShortErrorMessage, nameof(tooShortErrorMessage));
                _tooLongErrorMessage = Arguments.NotNull(tooLongErrorMessage, nameof(tooLongErrorMessage));
                _minLength = lengthRange.Min;
                _maxLength = lengthRange.Max;
            });
        }

        /// <summary>
        /// Indicates tha the given input must be trimmed (can not have white space characters at the biginning 
        /// or end). Same as <see cref="WithRequiresTrimmed(bool)"/> with <see langword="true"/>.
        /// </summary>
        /// <returns>Self</returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithRequiresTrimmed()
            => WithRequiresTrimmed(true, _invalidFormatErrorMessage ?? DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// Indicates if the given input must be trimmed (can not have white space characters at the biginning 
        /// or end).
        /// </summary>
        /// <param name="requiresTrimmed"></param>
        /// <returns>Self</returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithRequiresTrimmed(bool requiresTrimmed)
            => WithRequiresTrimmed(requiresTrimmed, _invalidFormatErrorMessage ?? DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// Indicates if the given input must be trimmed (can not have white space characters at the biginning 
        /// or end).
        /// </summary>
        /// <param name="requiresTrimmed"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithRequiresTrimmed(bool requiresTrimmed, Message invalidFormatErrorMessage)
        {
            return CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage, () =>
            {
                _requiresTrimmed = requiresTrimmed;
                if (requiresTrimmed)
                {
                    _allowLeadingWhiteSpace = _allowTrailingWhiteSpace = _allowWhiteSpacesOnly = false;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowLeadingWhiteSpace"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithAllowLeadingWhiteSpace(bool allowLeadingWhiteSpace)
            => WithAllowLeadingWhiteSpace(allowLeadingWhiteSpace, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowLeadingWhiteSpace"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithAllowLeadingWhiteSpace(bool allowLeadingWhiteSpace, Message invalidFormatErrorMessage)
            => CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => _allowLeadingWhiteSpace = allowLeadingWhiteSpace);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTrailingWhiteSpace"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithAllowTrailingWhiteSpace(bool allowTrailingWhiteSpace)
            => WithAllowTrailingWhiteSpace(allowTrailingWhiteSpace, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTrailingWhiteSpace"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithAllowTrailingWhiteSpace(bool allowTrailingWhiteSpace, Message invalidFormatErrorMessage)
        {
            return CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => _allowTrailingWhiteSpace = allowTrailingWhiteSpace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowWhiteSpacesOnly"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithAllowWhiteSpacesOnly(bool allowWhiteSpacesOnly)
            => CheckPreconditionsAndExecute(() => SetAllowWhiteSpacesOnly(allowWhiteSpacesOnly));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowWhiteSpacesOnly"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithAllowWhiteSpacesOnly(bool allowWhiteSpacesOnly, Message invalidFormatErrorMessage)
            => CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => SetAllowWhiteSpacesOnly(allowWhiteSpacesOnly));

        private void SetAllowWhiteSpacesOnly(bool allowWhiteSpacesOnly)
        {
            _allowWhiteSpacesOnly = allowWhiteSpacesOnly;

            if (allowWhiteSpacesOnly)
            {
                _allowLeadingWhiteSpace = _allowTrailingWhiteSpace = true;
                _requiresTrimmed = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithInvalidCharsPattern(string pattern)
            => WithInvalidCharsPattern(pattern, DefaultInvalidCharactersErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="invalidCharactersErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithInvalidCharsPattern(string pattern, Message invalidCharactersErrorMessage)
        {
            return CheckPreconditionsAndExecute(() =>
            {
                _invalidCharactersErrorMessage = Arguments.NotNull(invalidCharactersErrorMessage,
                    nameof(invalidCharactersErrorMessage));
                _invalidCharsRegex = new Regex(
                    Arguments.NotNull(pattern, nameof(pattern)),
                    RegexOptions.Compiled | RegexOptions.CultureInvariant, Regex.InfiniteMatchTimeout);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithInvalidCharsRegex(Regex regex)
            => WithInvalidCharsRegex(regex, DefaultInvalidCharactersErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="invalidCharactersErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithInvalidCharsRegex(Regex regex, Message invalidCharactersErrorMessage)
        {
            return CheckPreconditionsAndExecute(() =>
            {
                _invalidCharactersErrorMessage = Arguments.NotNull(invalidCharactersErrorMessage,
                    nameof(invalidCharactersErrorMessage));
                _invalidCharsRegex = Arguments.NotNull(regex, nameof(regex));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithValidFormatPattern(string pattern)
            => WithValidFormatPattern(pattern, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithValidFormatPattern(string pattern, Message invalidFormatErrorMessage)
        {
            return CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => _validFormatRegex = new Regex(Arguments.NotNull(pattern, nameof(pattern)),
                    RegexOptions.Compiled | RegexOptions.CultureInvariant, Regex.InfiniteMatchTimeout));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithValidFormatRegex(Regex regex)
            => WithValidFormatRegex(regex, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        public Builder WithValidFormatRegex(Regex regex, Message invalidFormatErrorMessage)
            => CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => _validFormatRegex = Arguments.NotNull(regex, nameof(regex)));

        private Builder CheckPreconditionsAndExecute(Action checkAndSet)
        {
            EnsureNotBuilt();

            checkAndSet();

            return this;
        }

        private Builder CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(
            Message invalidFormatErrorMessage, Action checkAndSet)
        {
            EnsureNotBuilt();

            _invalidFormatErrorMessage =
                Arguments.NotNull(invalidFormatErrorMessage, nameof(invalidFormatErrorMessage));

            checkAndSet();

            return this;
        }

        /// <summary>
        /// Validates input against all configured options, if all are met, creates the 
        /// <see cref="ConfigurableString"/> instance, otherwise throws exception.
        /// </summary>
        /// <param name="rawValue">Can not be <see langowrd="null"/></param>
        /// <returns><see cref="ConfigurableString"/> or throws exception</returns>
        public ConfigurableString Build(string? rawValue)
            => Build(rawValue, NoOpCustomParser);

        /// <summary>
        /// Validates input against all configured options, if all are met, creates the 
        /// <see cref="ConfigurableString"/> instance, otherwise throws exception.
        /// </summary>
        /// <param name="rawValue">Can not be <see langowrd="null"/></param>
        /// <param name="customParser">Can not be <see langowrd="null"/>. Applied after all options, as a final 
        /// validation.</param>
        /// <returns></returns>
        public ConfigurableString Build(string? rawValue, Action<string>? customParser)
        {
            EnsureNotBuilt();

            (string notNullValue, Action<string>? notNullCustomParser) =
                (Arguments.NotNull(rawValue, nameof(rawValue), _argumentNullErrorMessage.Value),
                Arguments.NotNull(customParser, nameof(customParser)));

            CheckLengthRange(notNullValue);

            CheckForTrimming(notNullValue);

            CheckForInvalidChars(notNullValue);

            CheckForValidFormat(notNullValue);

            CheckForWhiteSpaces(notNullValue);

            notNullCustomParser(notNullValue);

            _built = true;

            return new ConfigurableString(notNullValue, _comparisonStrategy);
        }

        private void EnsureNotBuilt() => State.IsFalse(_built, "Already built.");

        private void CheckLengthRange(string rawValue)
        {
            StringLengthRange.Validate(_minLength ?? StringLength.Min, _maxLength ?? StringLength.Max);

            if (_minLength is not null)
            {
                Arguments.GreaterThanOrEqualTo(rawValue.Length, _minLength.Value, nameof(rawValue),
                    _tooShortErrorMessage.Value);
            }

            if (_maxLength is not null)
            {
                Arguments.LessThanOrEqualTo(rawValue.Length, _maxLength.Value, nameof(rawValue),
                    _tooLongErrorMessage.Value);
            }
        }

        private void CheckForTrimming(string rawValue)
        {
            if (rawValue.Length == 0)
            {
                return;
            }

            if (_requiresTrimmed)
            {
                if (rawValue.IsNotTrimmed())
                {
                    throw new FormatException(_invalidFormatErrorMessage.Value);
                }
            }
            else
            {
                if (DoesNotAllowLeadingWhiteSpace && rawValue.HasLeadingWhiteSpace())
                {
                    throw new FormatException(_invalidFormatErrorMessage.Value);
                }

                if (DoesNotAllowTrailingWhiteSpace && rawValue.HasTrailingWhiteSpace())
                {
                    throw new FormatException(_invalidFormatErrorMessage.Value);
                }
            }
        }

        private void CheckForInvalidChars(string rawValue)
        {
            if (_invalidCharsRegex == null)
            {
                return;
            }

            if (_invalidCharsRegex.IsMatch(rawValue))
            {
                throw new FormatException(_invalidCharactersErrorMessage.Value);
            }
        }

        private void CheckForValidFormat(string rawValue)
        {
            if (_validFormatRegex == null)
            {
                return;
            }

            if (!_validFormatRegex.IsMatch(rawValue))
            {
                throw new FormatException(_invalidFormatErrorMessage.Value);
            }
        }

        private void CheckForWhiteSpaces(string rawValue)
        {
            bool CanNotBeEmpty() => _minLength is not null && _minLength.Value > 0;

            if (DoesNotAllowWhiteSpacesOnly && CanNotBeEmpty() && rawValue.IsWhiteSpaceOnly())
            {
                throw new FormatException(_invalidFormatErrorMessage.Value);
            }
        }
    }
}
