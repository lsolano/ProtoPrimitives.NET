using System.Globalization;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithInvalidCharsPatternMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidPatternMessage = new("Your pattern is buggy :(");

    private static readonly string[] PhrasesWithDigits = new[]{
        "Age 1", "Age 2", "Age 2",
    };

    public WithInvalidCharsPatternMessage(bool useSingleParamConstructor, bool useSingleMessage)
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
            => WithInvalidCharsPattern(builder, invalidCharsPattern, DefaultInvalidPatternMessage, sendErrorMessage),
            Throws.Nothing);
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
        WithInvalidCharsPattern(builder, "[0-9]", DefaultInvalidPatternMessage, sendErrorMessage);

        Assert.That(() => builder.Build(rawValueWithDigits), Throws.InstanceOf<FormatException>());
    }

    [Test]
    public void With_Null_Pattern_Throws_ArgumentNullException([Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithInvalidCharsPattern(builder, null!, DefaultInvalidPatternMessage, sendErrorMessage),
            Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("pattern"));
    }

    private static ConfigurableString.Builder WithInvalidCharsPattern(
        ConfigurableString.Builder builder,
        string pattern,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ?
            builder.WithInvalidCharsPattern(pattern, invalidFormatMsg) :
            builder.WithInvalidCharsPattern(pattern);
    }
}
