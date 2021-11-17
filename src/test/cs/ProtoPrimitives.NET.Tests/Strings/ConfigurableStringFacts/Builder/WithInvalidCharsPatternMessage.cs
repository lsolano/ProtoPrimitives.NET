using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithInvalidCharsPatternMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidPatternMessage = new("Your pattern is buggy :(");

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
