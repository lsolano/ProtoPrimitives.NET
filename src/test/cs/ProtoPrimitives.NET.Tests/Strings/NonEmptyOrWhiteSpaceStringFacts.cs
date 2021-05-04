using System;

using NUnit.Framework;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings
{
    internal static class NonEmptyOrWhiteSpaceStringFacts
    {
        private const string ValidSampleValue = "Hello World!!!";
        private static readonly Message CustomErrorMessage = new Message("Use me if something is not OK with rawValue");

        private static NonEmptyOrWhiteSpaceString Build(in string rawValue, in bool useCustomMessage)
        {
            return useCustomMessage
                ? new NonEmptyOrWhiteSpaceString(rawValue, CustomErrorMessage)
                : new NonEmptyOrWhiteSpaceString(rawValue);
        }

        internal sealed class ConstructorMessage : RawValueAndErrorMessageBaseFixture
        {
            private const string ParamName = "rawValue";
            private const string ErrorMessageParamName = "errorMessage";

            private readonly Message _expectedErrorMessage;

            public ConstructorMessage(bool useCustomMessage) : base(useCustomMessage)
            {
                _expectedErrorMessage =
                    useCustomMessage ? CustomErrorMessage : NonEmptyOrWhiteSpaceString.DefaultErrorMessage;
            }

            public void Rejects_Null()
                => Assert.That(() => Build(null, UseCustomMessage),
                    Throws.ArgumentNullException
                        .With.Message.StartWith(_expectedErrorMessage.Value)
                        .And.Message.Contain(ParamName)
                        .And.Property("ParamName").EqualTo(ParamName));

            [Test]
            public void Rejects_Invalid_Values(
                [Values(" ", "\n", "\r", "\t")] string rawValue)
                => Assert.That(() => Build(rawValue, UseCustomMessage), 
                    Throws.InstanceOf<FormatException>()
                        .With.Message.StartWith(_expectedErrorMessage.Value)
                        .And.Message.Contain(ParamName));

            [Test]
            public void Rejects_Empty_With_ArgumentOutOfRangeException() {
                Assert.That(() => Build(string.Empty, UseCustomMessage),
                    Throws.InstanceOf<ArgumentOutOfRangeException>()
                        .With.Message.StartWith(_expectedErrorMessage.Value)
                        .And.Message.Contain(ParamName));
            }

            [Test]
            public void Rejects_Null_Error_Message()
                => Assert.That(() => new NonEmptyOrWhiteSpaceString(ValidSampleValue, null), 
                    Throws.ArgumentNullException
                        .With.Message.StartWith(NonEmptyOrWhiteSpaceString.InvalidCustomErrorMessageMessage)
                        .And.Message.Contain(ErrorMessageParamName)
                        .And.Property(nameof(ArgumentNullException.ParamName)).EqualTo(ErrorMessageParamName));

            [TestCase("a")]
            [TestCase("ab")]
            [TestCase("a b")]
            [TestCase(" a b")]
            [TestCase("a b ")]
            [TestCase(" a b ")]
            [TestCase(ValidSampleValue)]
            public void Accepts_Valid_Values(string rawValue) => Assert.That(() => Build(rawValue, UseCustomMessage), Throws.Nothing);
        }

        internal  sealed class ValueProperty : RawValueAndErrorMessageBaseFixture
        {
            public ValueProperty(bool useCustomMessage) : base(useCustomMessage)
            {
            }

            [Test]
            public void Returns_Constructor_Provided_Value()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.Value, Is.EqualTo(ValidSampleValue));
            }
        }

        internal sealed class ToStringMessage : RawValueAndErrorMessageBaseFixture
        {
            public ToStringMessage(bool useCustomMessage) : base(useCustomMessage)
            {
            }

            [Test]
            public void Same_As_Raw_Value()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.ToString(), Is.EqualTo(ValidSampleValue));
            }
        }

        internal sealed class GetHashCodeMessage : RawValueAndErrorMessageBaseFixture
        {
            public GetHashCodeMessage(bool useCustomMessage) : base(useCustomMessage)
            {
            }

            [Test]
            public void Same_As_Raw_Value()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.GetHashCode(), Is.EqualTo(ValidSampleValue.GetHashCode()));
            }
        }

        internal sealed class EqualsMessage : RawValueAndErrorMessageBaseFixture
        {
            public EqualsMessage(bool useCustomMessage) : base(useCustomMessage)
            {
            }

            [Test]
            public void With_Null_Returns_False()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.Equals(null), Is.False);
            }

            [Test]
            public void With_Self_Returns_True()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.Equals(notEmptyOrWhiteSpaceString), Is.True);
            }

            [TestCase(ValidSampleValue)]
            [TestCase("peter")]
            [TestCase(true)]
            [TestCase(1.25)]
            public void With_Other_Types_Returns_False(in object other)
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.Equals(other), Is.False);
            }

            [Test]
            public void With_Same_Value_Returns_True()
            {
                (NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceStringA, NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceStringB)
                    = (Build(ValidSampleValue, UseCustomMessage), Build(ValidSampleValue, UseCustomMessage));

                Assert.That(notEmptyOrWhiteSpaceStringA.Equals(notEmptyOrWhiteSpaceStringB), Is.True);
            }

            [Test]
            public void With_Different_Values_Returns_False()
            {
                (NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceStringA, NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceStringB)
                    = (Build(ValidSampleValue, UseCustomMessage), Build(ValidSampleValue + "B", UseCustomMessage));

                Assert.That(notEmptyOrWhiteSpaceStringA.Equals(notEmptyOrWhiteSpaceStringB), Is.False);
            }
        }

        internal sealed class CompareToMessage : RawValueAndErrorMessageBaseFixture
        {
            public CompareToMessage(bool useCustomMessage) : base(useCustomMessage)
            {
            }

            [Test]
            public void With_Null_Returns_Positive()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.CompareTo(null), Is.GreaterThan(0));
            }

            [Test]
            public void With_Self_Returns_Zero()
            {
                NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceString = Build(ValidSampleValue, UseCustomMessage);

                Assert.That(notEmptyOrWhiteSpaceString.CompareTo(notEmptyOrWhiteSpaceString), Is.Zero);
            }

            [Test]
            public void Same_As_Raw_Value(
                [Values("hello", "world")] in string rawValueA,
                [Values("hello", "world")] in string rawValueB)
            {
                (NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceStringA,
                        NonEmptyOrWhiteSpaceString notEmptyOrWhiteSpaceStringB)
                    = (Build(rawValueA, UseCustomMessage), Build(rawValueB, UseCustomMessage));

                Assert.That(notEmptyOrWhiteSpaceStringA.CompareTo(notEmptyOrWhiteSpaceStringB),
                    Is.EqualTo(string.Compare(rawValueA, rawValueB, NonEmptyOrWhiteSpaceString.ComparisonStrategy)));
            }
        }
    }
}
