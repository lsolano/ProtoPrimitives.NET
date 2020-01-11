using System;
using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class WithRequiresTrimmedMessage : ValidConstructorArgumentsFixture
    {
        private static readonly Message DefaultInvalidFormatMessage = new Message("Can't have white-spaces leading or trailing.");

        public WithRequiresTrimmedMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_InvalidFormatMessage_ArgumentNullException([Values(false, true)] bool requiresTrimmed)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => WithRequiresTrimmed(builder, requiresTrimmed, null, true),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("invalidFormatErrorMessage"));
        }

        [Test]
        public void With_Valid_Input_Throws_Nothing([Values(false, true)] bool requiresTrimmed, [Values(false, true)] bool sendMessage)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => WithRequiresTrimmed(builder, requiresTrimmed, DefaultInvalidFormatMessage, sendMessage),
                Throws.Nothing);
        }

        [Test]
        public void Rejects_Leading_And_Trailing_White_Spaces_When_True(
            [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
            [Values(0, 1, 2)] int position,
            [Values(false, true)] bool requiresTrimmed,
            [Values(false, true)] bool sendMessage)
        {
            const string baseValue = "abc";
            string rawValue = position switch
            {
                0 => $"{whiteSpace}{baseValue}",
                1 => $"{baseValue}{whiteSpace}",
                _ => $"{whiteSpace}{baseValue}{whiteSpace}"
            };

            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            WithRequiresTrimmed(builder, requiresTrimmed, DefaultInvalidFormatMessage, sendMessage);

            if (requiresTrimmed)
            {
                Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
            }
            else
            {
                ConfigurableString str = builder.Build(rawValue);
                Assert.That(str.Value, Is.SameAs(rawValue));
            }
        }

        private static ConfigurableString.Builder WithRequiresTrimmed(
            in ConfigurableString.Builder builder,
            in bool requiresTrimmed,
            in Message invalidFormatMsg,
            in bool sendMessage)
        {
            return sendMessage ? builder.WithRequiresTrimmed(requiresTrimmed, invalidFormatMsg)
                : builder.WithRequiresTrimmed(requiresTrimmed);
            }
    }
}