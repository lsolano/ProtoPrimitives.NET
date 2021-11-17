using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class BuildMessage : ValidConstructorArgumentsFixture
{
    internal const string AlreadyBuiltErrorMessage = "Already built.";

    public BuildMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_Rawvalue_Throws_ArgumentNullException()
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        const string rawValue = null!;

        Assert.That(() => builder.Build(rawValue),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("rawValue")
                .With.Message.StartsWith(ArgNullErrorMessage.Value));
    }

    [Test]
    public void Allows_Build_After_Instantiation()
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        const string rawValue = "I'm a valid string";

        ConfigurableString str = builder.Build(rawValue);

        Assert.That(str.Value, Is.SameAs(rawValue));
    }

    [Test]
    public void Throws_If_Called_Twice()
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        _ = builder.Build("I'm a valid string");

        Assert.That(() => builder.Build("Some other value."),
            Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo(AlreadyBuiltErrorMessage));
    }

    internal static string ConcatAtPosition(WhiteSpacePosition position, string whiteSpace)
        => ConcatAtPosition(position, whiteSpace, "abc");

    internal static string ConcatAtPosition(WhiteSpacePosition position, string whiteSpace, string text)
    {
        return position switch
        {
            WhiteSpacePosition.None => text,
            WhiteSpacePosition.Leading => $"{whiteSpace}{text}",
            WhiteSpacePosition.Trailing => $"{text}{whiteSpace}",
            WhiteSpacePosition.Both => $"{whiteSpace}{text}{whiteSpace}",
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, "Not handled enumeration value.")
        };
    }

    internal static ConfigurableString.Builder Create(bool useSingleParamConstructor, bool useSingleMessage)
    => useSingleParamConstructor ?
        new ConfigurableString.Builder(ArgNullErrorMessage) :
        new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);

    internal static ConfigurableString.Builder Create(bool useSingleParamConstructor, Message theMessage,
        bool useSingleMessage)
        => useSingleParamConstructor ?
            new ConfigurableString.Builder(theMessage) :
            new ConfigurableString.Builder(theMessage, useSingleMessage);
}

internal enum WhiteSpacePosition { None, Leading, Trailing, Both }
