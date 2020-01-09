using System;
using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Numerics;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class WithMaxLengthMessage : ValidConstructorArgumentsFixture
    {
        private static readonly Message DefaultTooLongMessage = new Message("Very large");

        public WithMaxLengthMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_Throws_ArgumentNullException([Values(false, true)] bool doNotSendTooLongMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => SetMaxLength(doNotSendTooLongMessage, builder, null),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("maxLength"));
        }

        [Test]
        public void With_Valid_MaxLength_Throws_Nothing([Values(false, true)] bool doNotSendTooLongMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => SetMaxLength(doNotSendTooLongMessage, builder, new StringLength(64)), Throws.Nothing);
        }

        [Test]
        public void Uses_Set_MaxLength([Values("abc", "abcd")] in string rawValue, [Values(false, true)] in bool doNotSendTooLongMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            SetMaxLength(doNotSendTooLongMessage, builder, new StringLength(4));

            Assert.That(builder.Build(rawValue).Value, Is.SameAs(rawValue));
        }

        [Test]
        public void Rejects_RawValues_Larger_Than_MaxLength([Values("abc", "abcd")]  string rawValue, [Values(false, true)] in bool doNotSendTooLongMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            SetMaxLength(doNotSendTooLongMessage, builder, new StringLength(2));

            Assert.That(() => builder.Build(rawValue),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
        }

        private static void SetMaxLength(in bool doNotSendTooLongMessage, in ConfigurableString.Builder builder, in StringLength maxLength)
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
}