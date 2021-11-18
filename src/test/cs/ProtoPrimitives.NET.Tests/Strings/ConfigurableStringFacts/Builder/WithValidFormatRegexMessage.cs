using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithValidFormatRegexMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidPatternMessage = new("Your pattern is buggy :(");

    public WithValidFormatRegexMessage(bool useSingleParamConstructor, bool useSingleMessage)
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
            => WithValidFormatRegex(builder, invalidCharsPattern, DefaultInvalidPatternMessage, sendErrorMessage),
            Throws.Nothing);
    }

    [Test]
    public void With_Null_Pattern_Throws_ArgumentNullException([Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithValidFormatRegex(builder, null!, DefaultInvalidPatternMessage, sendErrorMessage),
            Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("pattern"));
    }

    [Test]
    public void With_All_Digits_Pattern_Rejects_Digits_With_Alpha_Throwing_FormatException(
       [Values("1a", "12U", "00a00")] string rawValue,
       [Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithValidFormatRegex(builder, "^[0-9]$", DefaultInvalidPatternMessage, sendErrorMessage);

        Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
    }

    [Test]
    public void With_All_Digits_Pattern_Accepts_Input_With_Digits_Only(
        [Values("1", "12", "00100")] string rawValue,
        [Values] bool sendErrorMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithValidFormatRegex(builder, "[0-9]", DefaultInvalidPatternMessage, sendErrorMessage);

        Assert.That(() => builder.Build(rawValue), Throws.Nothing);
    }

    private static ConfigurableString.Builder WithValidFormatRegex(
        ConfigurableString.Builder builder,
        string pattern,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ?
            builder.WithValidFormatRegex(new Regex(pattern), invalidFormatMsg) :
            builder.WithValidFormatRegex(new Regex(pattern));
    }
}
