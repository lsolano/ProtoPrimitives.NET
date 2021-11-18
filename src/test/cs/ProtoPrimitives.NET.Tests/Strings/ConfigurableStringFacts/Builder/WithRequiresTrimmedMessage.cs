using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;

using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithRequiresTrimmedMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidFormatMessage =
        new("Can't have white-spaces leading or trailing.");

    public WithRequiresTrimmedMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_InvalidFormatMessage_ArgumentNullException([Values] bool requiresTrimmed)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithRequiresTrimmed(builder, requiresTrimmed, null!, true, parameterless: false),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("invalidFormatErrorMessage"));
    }

    [Test]
    public void With_Valid_Input_Throws_Nothing([Values] bool requiresTrimmed, [Values] bool sendMessage,
        [Values] bool parameterless)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithRequiresTrimmed(builder, requiresTrimmed, DefaultInvalidFormatMessage, sendMessage,
            parameterless), Throws.Nothing);
    }

    [Test]
    public void With_Empty_And_False_Throws_Nothing([Values] bool sendMessage, [Values] bool parameterless)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithRequiresTrimmed(builder, requiresTrimmed: false, DefaultInvalidFormatMessage, sendMessage, parameterless);

        Assert.That(() => builder.Build(string.Empty), Throws.Nothing);
    }

    [Test]
    public void Rejects_Leading_And_Trailing_White_Spaces_When_True(
        [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
        [Values(WhiteSpacePosition.Leading, WhiteSpacePosition.Trailing, WhiteSpacePosition.Both)]
            WhiteSpacePosition position,
        [Values] bool sendMessage,
        [Values] bool parameterless)
    {
        Assume.That(!parameterless || !sendMessage, Is.True);

        string rawValue = ConcatAtPosition(position, whiteSpace);
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithRequiresTrimmed(builder, true, DefaultInvalidFormatMessage, sendMessage, parameterless);

        Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
    }

    [Test]
    public void Accepts_Leading_And_Trailing_White_Spaces_When_False(
        [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
        [Values] WhiteSpacePosition position,
        [Values] bool sendMessage)
    {
        string rawValue = ConcatAtPosition(position, whiteSpace);
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithRequiresTrimmed(builder, false, DefaultInvalidFormatMessage, sendMessage, parameterless: false);

        ConfigurableString str = builder.Build(rawValue);
        Assert.That(str.Value, Is.SameAs(rawValue));
    }

    [Test]
    public void Overrides_AllowLeadingWhiteSpace_Option_When_True(
        [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
        [Values(WhiteSpacePosition.Leading, WhiteSpacePosition.Both)] WhiteSpacePosition position,
        [Values] bool sendMessage, [Values] bool parameterless)
    {
        Assume.That(!parameterless || !sendMessage, Is.True);

        string rawValue = ConcatAtPosition(position, whiteSpace);
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        builder.WithAllowLeadingWhiteSpace(true);
        WithRequiresTrimmed(builder, true, DefaultInvalidFormatMessage, sendMessage, parameterless);

        Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
    }

    [Test]
    public void Rejects_Not_Trimmed_When_True()
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        builder.WithRequiresTrimmed();

        Assert.That(() => builder.Build(" Hello World "), Throws.InstanceOf<FormatException>());
    }

    [Test]
    public void Accepts_Trimmed_When_True()
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        builder.WithRequiresTrimmed();

        Assert.That(() => builder.Build("Hello World"), Throws.Nothing);
    }

    private static ConfigurableString.Builder WithRequiresTrimmed(
        ConfigurableString.Builder builder,
        bool requiresTrimmed,
        Message invalidFormatMsg,
        bool sendMessage,
        bool parameterless)
    {
        if (parameterless && requiresTrimmed && !sendMessage)
        {
            builder.WithRequiresTrimmed();
        }

        return sendMessage ? builder.WithRequiresTrimmed(requiresTrimmed, invalidFormatMsg)
        : builder.WithRequiresTrimmed(requiresTrimmed);
    }
}
