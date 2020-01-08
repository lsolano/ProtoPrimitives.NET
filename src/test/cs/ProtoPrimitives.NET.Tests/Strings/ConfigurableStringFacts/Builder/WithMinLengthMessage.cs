using System;
using NUnit.Framework;
using ProtoPrimitives.NET.Numerics;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class WithMinLengthMessage : ValidConstructorArgumentsFixture
    {
        public WithMinLengthMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_Throws_ArgumentNullException()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => builder.WithMinLength(null),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("minLength"));
        }

        [Test]
        public void With_Valid_MinLength_Throws_Nothing()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => builder.WithMinLength(new StringLength(64)), Throws.Nothing);
        }

        [TestCase("abc")]
        [TestCase("abcd")]
        public void Uses_Set_MinLength(in string rawValue)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            builder.WithMinLength(new StringLength(3));

            Assert.That(builder.Build(rawValue).Value, Is.SameAs(rawValue));
        }

        [TestCase("abc")]
        [TestCase("abcd")]
        public void Rejects_RawValues_Shorter_Than_MinLength(string rawValue)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            builder.WithMinLength(new StringLength(5));

            Assert.That(() => builder.Build(rawValue),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("rawValue")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(rawValue.Length));
        }
    }
}