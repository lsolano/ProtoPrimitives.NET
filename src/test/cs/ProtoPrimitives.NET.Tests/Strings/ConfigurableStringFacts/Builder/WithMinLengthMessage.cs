using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithMinLengthMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultTooShortMessage = new("Shorty");

    public WithMinLengthMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_Throws_ArgumentNullException([Values] bool doNotSendTooShortMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => SetMinLength(doNotSendTooShortMessage, builder, null!),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("minLength"));
    }

    [Test]
    public void With_Valid_MinLength_Throws_Nothing([Values] bool doNotSendTooShortMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => SetMinLength(doNotSendTooShortMessage, builder, new StringLength(64)), Throws.Nothing);
    }

    [Test]
    public void Uses_Set_MinLength([Values("abc", "abcd")] string rawValue,
        [Values] bool doNotSendTooShortMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        SetMinLength(doNotSendTooShortMessage, builder, new StringLength(3));

        Assert.That(builder.Build(rawValue).Value, Is.SameAs(rawValue));
    }

    [Test]
    public void Rejects_RawValues_Shorter_Than_MinLength([Values("abc", "abcd")] string rawValue,
        [Values] bool doNotSendTooShortMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        SetMinLength(doNotSendTooShortMessage, builder, new StringLength(5));

        Assert.That(() => builder.Build(rawValue),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
    }

    private static void SetMinLength(bool doNotSendTooShortMessage, ConfigurableString.Builder builder,
        StringLength minLength)
    {
        if (doNotSendTooShortMessage)
        {
            builder.WithMinLength(minLength);
        }
        else
        {
            builder.WithMinLength(minLength, DefaultTooShortMessage);
        }
    }
}
