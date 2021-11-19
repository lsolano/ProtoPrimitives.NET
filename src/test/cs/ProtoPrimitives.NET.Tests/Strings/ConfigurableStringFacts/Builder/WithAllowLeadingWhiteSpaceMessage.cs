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

    [Test]
    public void With_True_Accepts_Leading_And_No_White_Spaces_At_Begining(
        [Values("Hello", " World", "\tif(a == b){}", " Peter ")] string rawValue,
        [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage)
            .WithAllowTrailingWhiteSpace(true);
        WithAllowLeadingWhiteSpace(builder, true, DefaultInvalidFormatMessage, sendMessage);

        Assert.That(() => builder.Build(rawValue), Throws.Nothing);
    }

    [Test]
    public void With_False_Rejects_Leading_White_Spaces_At_Begining(
        [Values(" Hello", "\n\rWorld", "\tif(a == b){}", "\r\nPeter ")] string rawValue,
        [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage)
            .WithAllowTrailingWhiteSpace(true);
        WithAllowLeadingWhiteSpace(builder, false, DefaultInvalidFormatMessage, sendMessage);

        Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
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
