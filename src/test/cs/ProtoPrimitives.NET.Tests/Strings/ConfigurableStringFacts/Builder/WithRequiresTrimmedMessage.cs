using System;
using NUnit.Framework;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder
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
            [Values(WhiteSpacePosition.Leading, WhiteSpacePosition.Trailing, WhiteSpacePosition.Both)] WhiteSpacePosition position,
            [Values(false, true)] bool requiresTrimmed,
            [Values(false, true)] bool sendMessage)
        {
            string rawValue = ConcatWhiteSpacesAtPosition(position, whiteSpace);
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

        public enum WhiteSpacePosition { None, Leading, Trailing, Both }

        /*
         * Overrides: AllowLeadingWhiteSpace, AllowTrailingWhiteSpace, AllowWhiteSpaceOnly (true => false) 
         */
        [Test]
        public void Overrides_AllowLeadingWhiteSpace_Option_When_True(
             [Values(" ", "\t", "\n", "\r", "\t\r\n")] string whiteSpace,
             [Values(WhiteSpacePosition.Leading, WhiteSpacePosition.Both)] WhiteSpacePosition position,
             [Values(false, true)] bool sendMessage)
        {
            string rawValue = ConcatWhiteSpacesAtPosition(position, whiteSpace);
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            builder.WithAllowLeadingWhiteSpace(true);
            WithRequiresTrimmed(builder, true, DefaultInvalidFormatMessage, sendMessage);

            Assert.That(() => builder.Build(rawValue), Throws.InstanceOf<FormatException>());
        }

        private static ConfigurableString.Builder WithRequiresTrimmed(
            in ConfigurableString.Builder builder,
            in bool requiresTrimmed,
            in Message invalidFormatMsg,
            in bool sendMessage) => sendMessage ? builder.WithRequiresTrimmed(requiresTrimmed, invalidFormatMsg)
                : builder.WithRequiresTrimmed(requiresTrimmed);

        private static string ConcatWhiteSpacesAtPosition(in WhiteSpacePosition position, in string whiteSpace)
        {
            const string baseValue = "abc";
            return position switch
            {
                WhiteSpacePosition.None => baseValue,
                WhiteSpacePosition.Leading => $"{whiteSpace}{baseValue}",
                WhiteSpacePosition.Trailing => $"{baseValue}{whiteSpace}",
                WhiteSpacePosition.Both => $"{whiteSpace}{baseValue}{whiteSpace}",
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, "Not handled enumeration value.")
            };
        }
    }
}