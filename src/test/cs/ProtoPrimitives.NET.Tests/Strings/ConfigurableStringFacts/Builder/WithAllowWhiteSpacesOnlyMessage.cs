using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;

using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithAllowWhiteSpacesOnlyMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultInvalidFormatMessage =
        new("Can't have white-spaces leading or trailing.");

    public WithAllowWhiteSpacesOnlyMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_InvalidFormatMessage_ArgumentNullException([Values] bool requiresTrimmed)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithAllowWhiteSpacesOnly(builder, requiresTrimmed, null!, true),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("invalidFormatErrorMessage"));
    }

    [Test]
    public void With_Valid_Input_Throws_Nothing([Values] bool requiresTrimmed, [Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => WithAllowWhiteSpacesOnly(builder, requiresTrimmed, DefaultInvalidFormatMessage, sendMessage),
            Throws.Nothing);
    }

    [Test]
    public void Rejects_Empty_When_False([Values] bool sendMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        WithAllowWhiteSpacesOnly(builder, false, DefaultInvalidFormatMessage, sendMessage);

        Assert.That(() => builder.Build("   "), Throws.InstanceOf<FormatException>());
    }

    // [Test]
    // public void Accepts_Leading_And_Trailing_White_Spaces_When_False(
    //     [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
    //     [Values] WhiteSpacePosition position,
    //     [Values] bool sendMessage)
    // {
    //     string rawValue = ConcatWhiteSpacesAtPosition(position, whiteSpace);
    //     ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
    //     WithAllowWhiteSpacesOnly(builder, false, DefaultInvalidFormatMessage, sendMessage, parameterless: false);

    //     ConfigurableString str = builder.Build(rawValue);
    //     Assert.That(str.Value, Is.SameAs(rawValue));
    // }

    // public enum WhiteSpacePosition { None, Leading, Trailing, Both }

    // /*
    //  * Overrides: AllowLeadingWhiteSpace, AllowTrailingWhiteSpace, AllowWhiteSpaceOnly (true => false) 
    //  */
    // [Test]
    // public void Overrides_AllowLeadingWhiteSpace_Option_When_True(
    //      [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
    //      [Values(WhiteSpacePosition.Leading, WhiteSpacePosition.Both)] WhiteSpacePosition position,
    //      [Values] bool sendMessage)
    // {
    //     string rawValue = ConcatWhiteSpacesAtPosition(position, whiteSpace);
    //     ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
    //     builder.WithAllowLeadingWhiteSpace(true);
    //     WithAllowWhiteSpacesOnly(builder, true, DefaultInvalidFormatMessage, sendMessage);

    //     Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
    // }

    private static ConfigurableString.Builder WithAllowWhiteSpacesOnly(
        ConfigurableString.Builder builder,
        bool allowWhiteSpacesOnly,
        Message invalidFormatMsg,
        bool sendMessage)
    {
        return sendMessage ? builder.WithAllowWhiteSpacesOnly(allowWhiteSpacesOnly, invalidFormatMsg)
        : builder.WithAllowWhiteSpacesOnly(allowWhiteSpacesOnly);
    }

    // private static string ConcatWhiteSpacesAtPosition(WhiteSpacePosition position, string whiteSpace)
    // {
    //     const string baseValue = "abc";
    //     return position switch
    //     {
    //         WhiteSpacePosition.None => baseValue,
    //         WhiteSpacePosition.Leading => $"{whiteSpace}{baseValue}",
    //         WhiteSpacePosition.Trailing => $"{baseValue}{whiteSpace}",
    //         WhiteSpacePosition.Both => $"{whiteSpace}{baseValue}{whiteSpace}",
    //         _ => throw new ArgumentOutOfRangeException(nameof(position), position, "Not handled enumeration value.")
    //     };
    // }
}
