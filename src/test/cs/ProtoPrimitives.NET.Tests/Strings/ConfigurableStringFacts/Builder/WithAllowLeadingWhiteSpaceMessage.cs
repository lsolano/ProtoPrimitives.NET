using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithAllowLeadingWhiteSpaceMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidFormatMessage =
        new("Can't have white-spaces leading or trailing.");

    public WithAllowLeadingWhiteSpaceMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_InvalidFormatMessage_ArgumentNullException([Values] bool allowLeadingWhiteSpace)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithAllowLeadingWhiteSpace(builder, allowLeadingWhiteSpace, null!, true),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("invalidFormatErrorMessage"));
    }

    [Test]
    public void With_Valid_Input_Throws_Nothing([Values] bool allowLeadingWhiteSpace, [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(()
            => WithAllowLeadingWhiteSpace(builder, allowLeadingWhiteSpace, DefaultInvalidFormatMessage, sendMessage),
            Throws.Nothing);
    }

    private static ConfigurableString.Builder WithAllowLeadingWhiteSpace(
        ConfigurableString.Builder builder,
        bool allowLeadingWhiteSpace,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ?
            builder.WithAllowLeadingWhiteSpace(allowLeadingWhiteSpace, invalidFormatMsg) :
            builder.WithAllowLeadingWhiteSpace(allowLeadingWhiteSpace);
    }
}
