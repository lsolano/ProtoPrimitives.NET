using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithAllowTrailingWhiteSpaceMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidFormatMessage =
        new("Can't have white-spaces leading or trailing.");

    public WithAllowTrailingWhiteSpaceMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_InvalidFormatMessage_ArgumentNullException([Values] bool allowTrailingWhiteSpace)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithAllowTrailingWhiteSpace(builder, allowTrailingWhiteSpace, null!, true),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("invalidFormatErrorMessage"));
    }

    [Test]
    public void With_Valid_Input_Throws_Nothing([Values] bool allowTrailingWhiteSpace, [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(()
            => WithAllowTrailingWhiteSpace(builder, allowTrailingWhiteSpace, DefaultInvalidFormatMessage, sendMessage),
            Throws.Nothing);
    }

    [Test]
    public void With_True_Accepts_Trailing_And_No_White_Spaces_At_End(
        [Values("Hello", "World ", "if(a == b){}\t", " Peter ")] string rawValue,
        [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage)
            .WithAllowLeadingWhiteSpace(true);
        WithAllowTrailingWhiteSpace(builder, true, DefaultInvalidFormatMessage, sendMessage);

        Assert.That(() => builder.Build(rawValue), Throws.Nothing);
    }

    [Test]
    public void With_False_Rejects_Trailing_White_Spaces_At_End(
        [Values("Hello ", "World\n\r", "if(a == b){}\t", "Peter\r\n")] string rawValue,
        [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage)
            .WithAllowLeadingWhiteSpace(true);
        WithAllowTrailingWhiteSpace(builder, false, DefaultInvalidFormatMessage, sendMessage);

        Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
    }

    private static ConfigurableString.Builder WithAllowTrailingWhiteSpace(
        ConfigurableString.Builder builder,
        bool allowTrailingWhiteSpace,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ?
            builder.WithAllowTrailingWhiteSpace(allowTrailingWhiteSpace, invalidFormatMsg) :
            builder.WithAllowTrailingWhiteSpace(allowTrailingWhiteSpace);
    }
}
