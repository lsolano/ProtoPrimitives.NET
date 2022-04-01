using System.Diagnostics.CodeAnalysis;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;

namespace Triplex.ProtoDomainPrimitives.Strings;

/// <summary>
/// <para>
/// String wrapper with several format and structural validations upon construction.
/// Use <see cref="Builder"/> to create instances of this class.
/// </para>
/// <para>
/// Be aware that wrapped value van not be <see langword="null"/> above all other validations.
/// </para>
/// </summary>
public sealed class ConfigurableString : AbstractDomainPrimitive<string>, IComparable<ConfigurableString>,
    IEquatable<ConfigurableString>
{
    private static readonly Message FallbackMessage = new("Invalid input.");

    private readonly StringComparison _comparisonStrategy;

    private ConfigurableString([NotNull] string rawValue, [NotNull] StringComparison comparisonStrategy) :
        base(rawValue, FallbackMessage, (val, _) => val!) => _comparisonStrategy = comparisonStrategy;

    /// <summary>
    /// Checks for equality using the strategy specified by builder <see cref="Builder.WithComparisonStrategy"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as ConfigurableString);

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
            .SelfIsEqualsTo(this, other, o => Value.Equals(o.Value, _comparisonStrategy));

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
        public Builder([NotNull] Message argumentNullErrorMessage) : this(argumentNullErrorMessage, false)
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
        public Builder([NotNull] Message argumentNullErrorMessage, bool useSingleMessage)
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
        [return: NotNull]
        public Builder WithComparisonStrategy([NotNull] StringComparison comparisonStrategy)
            => CheckPreconditionsAndExecute(() => _comparisonStrategy =
                Arguments.ValidEnumerationMember(comparisonStrategy, nameof(comparisonStrategy)));

        /// <summary>
        /// Sets the minimum allowed length.
        /// </summary>
        /// <param name="minLength">Can not be <see langword="null"/>.</param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithMinLength([NotNull] StringLength minLength)
            => CheckPreconditionsAndExecute(() => _minLength = Arguments.NotNull(minLength, nameof(minLength)));

        /// <summary>
        /// Sets the minimum allowed length and associated violation message.
        /// </summary>
        /// <param name="minLength">Can not be <see langword="null"/>.</param>
        /// <param name="tooShortErrorMessage">Can not be <see langword="null"/>.</param>
        /// <returns>Self</returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithMinLength([NotNull] StringLength minLength, [NotNull] Message tooShortErrorMessage)
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
        [return: NotNull]
        public Builder WithMaxLength([NotNull] StringLength maxLength)
            => CheckPreconditionsAndExecute(() => _maxLength = Arguments.NotNull(maxLength, nameof(maxLength)));

        /// <summary>
        /// Sets the maximum allowed length and associated violation message.
        /// </summary>
        /// <param name="maxLength">Can not be <see langword="null"/>.</param>
        /// <param name="tooLongErrorMessage">Can not be <see langword="null"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithMaxLength([NotNull] StringLength maxLength, [NotNull] Message tooLongErrorMessage)
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
        [return: NotNull]
        public Builder WithLengthRange([NotNull] StringLengthRange lengthRange, [NotNull] Message tooShortErrorMessage, 
            [NotNull] Message tooLongErrorMessage)
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
        [return: NotNull]
        public Builder WithRequiresTrimmed()
            => WithRequiresTrimmed(true, _invalidFormatErrorMessage);

        /// <summary>
        /// Indicates if the given input must be trimmed (can not have white space characters at the biginning 
        /// or end).
        /// </summary>
        /// <param name="requiresTrimmed"></param>
        /// <returns>Self</returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithRequiresTrimmed(bool requiresTrimmed)
            => WithRequiresTrimmed(requiresTrimmed, _invalidFormatErrorMessage);

        /// <summary>
        /// Indicates if the given input must be trimmed (can not have white space characters at the biginning 
        /// or end).
        /// </summary>
        /// <param name="requiresTrimmed"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">When any parameter is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithRequiresTrimmed(bool requiresTrimmed, [NotNull] Message invalidFormatErrorMessage)
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
        [return: NotNull]
        public Builder WithAllowLeadingWhiteSpace(bool allowLeadingWhiteSpace)
            => WithAllowLeadingWhiteSpace(allowLeadingWhiteSpace, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowLeadingWhiteSpace"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithAllowLeadingWhiteSpace(bool allowLeadingWhiteSpace, [NotNull] Message invalidFormatErrorMessage)
            => CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => _allowLeadingWhiteSpace = allowLeadingWhiteSpace);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTrailingWhiteSpace"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithAllowTrailingWhiteSpace(bool allowTrailingWhiteSpace)
            => WithAllowTrailingWhiteSpace(allowTrailingWhiteSpace, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTrailingWhiteSpace"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithAllowTrailingWhiteSpace(bool allowTrailingWhiteSpace, [NotNull] Message invalidFormatErrorMessage)
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
        [return: NotNull]
        public Builder WithAllowWhiteSpacesOnly(bool allowWhiteSpacesOnly)
            => CheckPreconditionsAndExecute(() => SetAllowWhiteSpacesOnly(allowWhiteSpacesOnly));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowWhiteSpacesOnly"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithAllowWhiteSpacesOnly(bool allowWhiteSpacesOnly, [NotNull] Message invalidFormatErrorMessage)
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
        [return: NotNull]
        public Builder WithInvalidCharsPattern([NotNull] string pattern)
            => WithInvalidCharsPattern(pattern, DefaultInvalidCharactersErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="invalidCharactersErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithInvalidCharsPattern([NotNull] string pattern, [NotNull] Message invalidCharactersErrorMessage)
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
        [return: NotNull]
        public Builder WithInvalidCharsRegex([NotNull] Regex regex)
            => WithInvalidCharsRegex(regex, DefaultInvalidCharactersErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="invalidCharactersErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithInvalidCharsRegex([NotNull] Regex regex, [NotNull] Message invalidCharactersErrorMessage)
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
        [return: NotNull]
        public Builder WithValidFormatPattern([NotNull] string pattern)
            => WithValidFormatPattern(pattern, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithValidFormatPattern([NotNull] string pattern, [NotNull] Message invalidFormatErrorMessage)
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
        [return: NotNull]
        public Builder WithValidFormatRegex([NotNull] Regex regex)
            => WithValidFormatRegex(regex, DefaultInvalidFormatErrorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="invalidFormatErrorMessage"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If already built.</exception>
        [return: NotNull]
        public Builder WithValidFormatRegex([NotNull] Regex regex, [NotNull] Message invalidFormatErrorMessage)
            => CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(invalidFormatErrorMessage,
                () => _validFormatRegex = Arguments.NotNull(regex, nameof(regex)));

        [return: NotNull]
        private Builder CheckPreconditionsAndExecute(Action checkAndSet)
        {
            EnsureNotBuilt();

            checkAndSet();

            return this;
        }

        [return: NotNull]
        private Builder CheckPreconditionsTrySetInvalidFormatErrorMessageAndExecute(
            [NotNull] Message invalidFormatErrorMessage, Action checkAndSet)
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
        [return: NotNull]
        public ConfigurableString Build([NotNull] string? rawValue)
            => Build(rawValue, NoOpCustomParser);

        /// <summary>
        /// Validates input against all configured options, if all are met, creates the 
        /// <see cref="ConfigurableString"/> instance, otherwise throws exception.
        /// </summary>
        /// <param name="rawValue">Can not be <see langowrd="null"/></param>
        /// <param name="customParser">Can not be <see langowrd="null"/>. Applied after all options, as a final 
        /// validation.</param>
        /// <returns></returns>
        [return: NotNull]
        public ConfigurableString Build([NotNull] string? rawValue, [NotNull] Action<string>? customParser)
        {
            EnsureNotBuilt();

            (string notNullValue, Action<string> notNullCustomParser) =
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
            else if (_requiresTrimmed)
            {
                ThrowIfNotFullyTrimmed(rawValue);
            }
            else
            {
                ThrowIfInvalidLeadingOrTrailingWhiteSpacesFound(rawValue);
            }
        }

        private void ThrowIfNotFullyTrimmed(string rawValue)
        {
            if (rawValue.IsNotTrimmed())
            {
                throw new FormatException(_invalidFormatErrorMessage.Value);
            }
        }

        private void ThrowIfInvalidLeadingOrTrailingWhiteSpacesFound(string rawValue)
        {
            bool hasLeadingOrTrailingWhiteSpace =
                (DoesNotAllowLeadingWhiteSpace && rawValue.HasLeadingWhiteSpace())
                || (DoesNotAllowTrailingWhiteSpace && rawValue.HasTrailingWhiteSpace());

            if (hasLeadingOrTrailingWhiteSpace)
            {
                throw new FormatException(_invalidFormatErrorMessage.Value);
            }
        }

        private void CheckForInvalidChars(string rawValue)
        {
            if (_invalidCharsRegex is null || !_invalidCharsRegex.IsMatch(rawValue))
            {
                return;
            }

            throw new FormatException(_invalidCharactersErrorMessage.Value);
        }

        private void CheckForValidFormat(string rawValue)
        {
            if (_validFormatRegex is null || _validFormatRegex.IsMatch(rawValue))
            {
                return;
            }

            throw new FormatException(_invalidFormatErrorMessage.Value);
        }

        private void CheckForWhiteSpaces(string rawValue)
        {
            if (DoesNotAllowWhiteSpacesOnly && rawValue.IsWhiteSpaceOnly())
            {
                throw new FormatException(_invalidFormatErrorMessage.Value);
            }
        }
    }
}
