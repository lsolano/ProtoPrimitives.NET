using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;

using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithAllowWhiteSpacesOnlyMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidFormatMessage = new("Can't have white-spaces leading or trailing.");

    public WithAllowWhiteSpacesOnlyMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_InvalidFormatMessage_ArgumentNullException([Values] bool allowWhiteSpacesOnly)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithAllowWhiteSpacesOnly(builder, allowWhiteSpacesOnly, null!, true),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("invalidFormatErrorMessage"));
    }

    [Test]
    public void With_Valid_Input_Throws_Nothing([Values] bool allowWhiteSpacesOnly, [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithAllowWhiteSpacesOnly(builder, allowWhiteSpacesOnly, DefaultInvalidFormatMessage,
            sendMessage), Throws.Nothing);
    }

    [Test]
    public void Rejects_Empty_When_False([Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithAllowWhiteSpacesOnly(builder, false, DefaultInvalidFormatMessage, sendMessage);

        Assert.That(() => builder.Build("   "), Throws.InstanceOf<FormatException>());
    }

    private static ConfigurableString.Builder WithAllowWhiteSpacesOnly(
        ConfigurableString.Builder builder,
        bool allowWhiteSpacesOnly,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ?
            builder.WithAllowWhiteSpacesOnly(allowWhiteSpacesOnly, invalidFormatMsg) :
            builder.WithAllowWhiteSpacesOnly(allowWhiteSpacesOnly);
    }
}
