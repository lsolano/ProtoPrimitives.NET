using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithInvalidCharsRegexMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidPatternMessage = new("Your pattern is buggy :(");

    public WithInvalidCharsRegexMessage(bool useSingleParamConstructor, bool useSingleMessage)
            : base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Valid_Values_Throws_Nothing(
        [Values("", ".*", "[áéíóú]")] string invalidCharsPattern,
        [Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(()
            => WithInvalidCharsRegex(builder, invalidCharsPattern, DefaultInvalidPatternMessage, sendErrorMessage),
            Throws.Nothing);
    }

    [Test]
    public void With_Null_Pattern_Throws_ArgumentNullException([Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithInvalidCharsRegex(builder, null!, DefaultInvalidPatternMessage, sendErrorMessage),
            Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("pattern"));
    }

    [Test]
    public void With_All_Digits_Pattern_Rejects_Digits_Throwing_FormatException(
        [Range(-10, 10)] int digits,
        [Values("0", "00")] string format,
        [Values] bool sendErrorMessage)
    {
        string formattedDigits = digits.ToString(format, CultureInfo.InvariantCulture);
        string rawValueWithDigits = string.Format("My age is {0}, and yours?", formattedDigits);

        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithInvalidCharsRegex(builder, "[0-9]", DefaultInvalidPatternMessage, sendErrorMessage);

        Assert.That(() => builder.Build(rawValueWithDigits), Throws.InstanceOf<FormatException>());
    }

    [Test]
    public void With_All_Digits_Pattern_Accepts_Input_Without_Digits([Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithInvalidCharsRegex(builder, "[0-9]", DefaultInvalidPatternMessage, sendErrorMessage);

        Assert.That(() => builder.Build("I'm five (V) years old."), Throws.Nothing);
    }

    private static ConfigurableString.Builder WithInvalidCharsRegex(
        ConfigurableString.Builder builder,
        string pattern,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ?
            builder.WithInvalidCharsRegex(new Regex(pattern), invalidFormatMsg) :
            builder.WithInvalidCharsRegex(new Regex(pattern));
    }
}
