using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.BuildMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder;

internal sealed class WithMaxLengthMessage : ValidConstructorArgumentsFixture
{
    private static readonly Message DefaultTooLongMessage = new("Very large");

    public WithMaxLengthMessage(bool useSingleParamConstructor, bool useSingleMessage) :
        base(useSingleParamConstructor, useSingleMessage)
    {
    }

    [Test]
    public void With_Null_Throws_ArgumentNullException([Values] bool doNotSendTooLongMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => SetMaxLength(doNotSendTooLongMessage, builder, null!),
            Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("maxLength"));
    }

    [Test]
    public void With_Valid_MaxLength_Throws_Nothing([Values] bool doNotSendTooLongMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

        Assert.That(() => SetMaxLength(doNotSendTooLongMessage, builder, new StringLength(64)), Throws.Nothing);
    }

    [Test]
    public void Uses_Set_MaxLength([Values("abc", "abcd")] string rawValue, [Values] bool doNotSendTooLongMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        SetMaxLength(doNotSendTooLongMessage, builder, new StringLength(4));

        Assert.That(builder.Build(rawValue).Value, Is.SameAs(rawValue));
    }

    [Test]
    public void Rejects_RawValues_Larger_Than_MaxLength([Values("abc", "abcd")] string rawValue,
        [Values] bool doNotSendTooLongMessage)
    {
        ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
        SetMaxLength(doNotSendTooLongMessage, builder, new StringLength(2));

        Assert.That(() => builder.Build(rawValue),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
    }

    private static void SetMaxLength(bool doNotSendTooLongMessage, ConfigurableString.Builder builder,
        StringLength maxLength)
    {
        if (doNotSendTooLongMessage)
        {
            builder.WithMaxLength(maxLength);
        }
        else
        {
            builder.WithMaxLength(maxLength, DefaultTooLongMessage);
        }
    }
}
