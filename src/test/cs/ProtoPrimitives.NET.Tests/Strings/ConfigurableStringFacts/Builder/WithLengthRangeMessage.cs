using System;
using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Numerics;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class WithLengthRangeMessage : ValidConstructorArgumentsFixture
    {
        private static readonly StringLengthRange SomeRange = new StringLengthRange(new StringLength(32), new StringLength(64));
        private static readonly Message DefaultTooLongMessage = new Message("Very large");

        public WithLengthRangeMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_Short_Message_ArgumentNullException()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => WithLengthRange(builder, SomeRange, null, DefaultTooLongMessage),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("tooShortErrorMessage"));
        }

        [Test]
        public void With_Null_Long_Message_ArgumentNullException()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => WithLengthRange(builder, SomeRange, DefaultTooLongMessage, null),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("tooLongErrorMessage"));
        }

        [Test]
        public void With_Null_Range_Throws_ArgumentNullException()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => WithLengthRange(builder, null, DefaultTooLongMessage, DefaultTooLongMessage),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("lengthRange"));
        }

        [Test]
        public void With_Valid_LengthRange_Throws_Nothing()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => WithLengthRange(builder, SomeRange, DefaultTooLongMessage, DefaultTooLongMessage),
                Throws.Nothing);
        }

        [Test]
        public void Uses_Set_LengthRange([Values("ab", "abc", "abcd")] in string rawValue)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            WithLengthRange(builder, new StringLengthRange(new StringLength(2), new StringLength(4)), DefaultTooLongMessage, DefaultTooLongMessage);

            Assert.That(builder.Build(rawValue).Value, Is.SameAs(rawValue));
        }

        [Test]
        public void Rejects_RawValues_Larger_Than_MaxLength([Values("abc", "abcd")]  string rawValue)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            WithLengthRange(builder, new StringLengthRange(new StringLength(1), new StringLength(2)), DefaultTooLongMessage, DefaultTooLongMessage);

            Assert.That(() => builder.Build(rawValue),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
        }

        [Test]
        public void Rejects_RawValues_Shorter_Than_MinLength([Values("a", "ab")]  string rawValue)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            WithLengthRange(builder, new StringLengthRange(new StringLength(3), new StringLength(5)), DefaultTooLongMessage, DefaultTooLongMessage);

            Assert.That(() => builder.Build(rawValue),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
        }

        private static void WithLengthRange(in ConfigurableString.Builder builder, in StringLengthRange lengthRange, in Message tooShortMessage, in Message tooLongMessage)
        {
            builder.WithLengthRange(lengthRange, tooShortMessage, tooLongMessage);
        }
    }
}