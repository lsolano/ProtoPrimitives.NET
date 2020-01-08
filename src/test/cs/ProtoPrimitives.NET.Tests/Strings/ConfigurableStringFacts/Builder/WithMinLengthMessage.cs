using System;
using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Numerics;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class WithMinLengthMessage : ValidConstructorArgumentsFixture
    {
        private static readonly Message DefaultTooShortMessage = new Message("Shorty");

        public WithMinLengthMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_Throws_ArgumentNullException([Values(false, true)] bool doNotSendTooShortMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => SetMinLength(doNotSendTooShortMessage, builder, null),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("minLength"));
        }

        [Test]
        public void With_Valid_MinLength_Throws_Nothing([Values(false, true)] bool doNotSendTooShortMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => SetMinLength(doNotSendTooShortMessage, builder, new StringLength(64)), Throws.Nothing);
        }

        [Test]
        public void Uses_Set_MinLength([Values("abc", "abcd")] in string rawValue, [Values(false, true)] in bool doNotSendTooShortMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            SetMinLength(doNotSendTooShortMessage, builder, new StringLength(3));

            Assert.That(builder.Build(rawValue).Value, Is.SameAs(rawValue));
        }

        [Test]
        public void Rejects_RawValues_Shorter_Than_MinLength([Values("abc", "abcd")]  string rawValue, [Values(false, true)] in bool doNotSendTooShortMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            SetMinLength(doNotSendTooShortMessage, builder, new StringLength(5));

            Assert.That(() => builder.Build(rawValue),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
        }

        private static void SetMinLength(in bool doNotSendTooShortMessage, in ConfigurableString.Builder builder, in StringLength minLength)
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
}