using NUnit.Framework;
using NUnit.Framework.Constraints;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Temporal;
using System;
using System.Collections.Generic;
using System.Linq;
using static Triplex.ProtoDomainPrimitives.Tests.Temporal.TemporalExtensions;

namespace Triplex.ProtoDomainPrimitives.Tests.Temporal
{
    internal static class FutureTimestampFacts
    {
        [TestFixture]
        internal sealed class ConstructorMessage
        {
            private static readonly IEnumerable<TimeMagnitude> Magnitudes = Enum.GetValues(typeof(TimeMagnitude)).Cast<TimeMagnitude>().ToList();

            private const string ParamName = "rawValue";
            private static readonly Message CustomErrorMessage = new Message("Some custom error message");

            [Test]
            public void Rejects_Past_Values_With_Default_Error_Message(
                [ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, -1);

                Assert.That(() => new FutureTimestamp(rawValue),
                            BuildArgumentOutOfRangeExceptionConstraint(rawValue, FutureTimestamp.DefaultErrorMessage));
            }

            [Test]
            public void Rejects_Past_Values_With_Custom_Error_Message(
                [ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, -1);

                Assert.That(() => new FutureTimestamp(rawValue, CustomErrorMessage),
                            BuildArgumentOutOfRangeExceptionConstraint(rawValue, CustomErrorMessage));
            }

            [Test]
            public void Rejects_Present_Value_With_Default_Error_Message()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow;
                Assert.That(() => new FutureTimestamp(rawValue), BuildArgumentOutOfRangeExceptionConstraint(rawValue, FutureTimestamp.DefaultErrorMessage));
            }

            [Test]
            public void Rejects_Present_Value_With_Custom_Error_Message()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow;
                Assert.That(() => new FutureTimestamp(rawValue, CustomErrorMessage), BuildArgumentOutOfRangeExceptionConstraint(rawValue, CustomErrorMessage));
            }

            private static IResolveConstraint BuildArgumentOutOfRangeExceptionConstraint(in DateTimeOffset rawValue, in Message errorMessage)
            {
                return Throws.InstanceOf<ArgumentOutOfRangeException>().With.Message.StartsWith(errorMessage.Value)
                                                                       .And.Message.Contains(ParamName)
                                                                       .And.Message.Contains(rawValue.ToString());
            }

            [Test]
            public void Accepts_Future_Values_With_Default_Error_Message([ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, 100);

                Assert.That(() => new FutureTimestamp(rawValue), Throws.Nothing);
            }

            [Test]
            public void Accepts_Future_Values_With_Custom_Error_Message([ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, 100);

                Assert.That(() => new FutureTimestamp(rawValue, CustomErrorMessage), Throws.Nothing);
            }
        }

        [TestFixture]
        internal sealed class ValueProperty
        {
            [Test]
            public void Returns_Constructor_Provided_Value()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.AddMinutes(5);
                var futureTimestamp = new FutureTimestamp(rawValue);

                Assert.That(futureTimestamp.Value, Is.EqualTo(rawValue));
            }
        }

        [TestFixture]
        internal sealed class ToStringMessage
        {
            [Test]
            public void Same_As_Raw_Value()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.AddMinutes(5);
                var futureTimestamp = new FutureTimestamp(rawValue);

                Assert.That(futureTimestamp.ToString(), Is.EqualTo(rawValue.ToString()));
            }
        }

        [TestFixture]
        internal sealed class GetHashCodeMessage
        {
            [Test]
            public void Same_As_Raw_Value()
            {
                (FutureTimestamp futureTimestamp, DateTimeOffset rawValue) = CreateWithFiveMinutesInTheFuture();

                Assert.That(futureTimestamp.GetHashCode(), Is.EqualTo(rawValue.GetHashCode()));
            }
        }

        [TestFixture]
        internal sealed class EqualsMessage
        {
            [Test]
            public void With_Null_Returns_False()
            {
                (FutureTimestamp futureTimestamp, _) = CreateWithFiveMinutesInTheFuture();

                Assert.That(futureTimestamp.Equals(null), Is.False);
            }

            [Test]
            public void With_Self_Returns_True()
            {
                (FutureTimestamp futureTimestamp, _) = CreateWithFiveMinutesInTheFuture();

                Assert.That(futureTimestamp.Equals(futureTimestamp), Is.True);
            }

            [TestCase(1024)]
            [TestCase("peter")]
            [TestCase(true)]
            [TestCase(1.25)]
            public void With_Other_Types_Returns_False(in object other)
            {
                (FutureTimestamp futureTimestamp, _) = CreateWithFiveMinutesInTheFuture();

                Assert.That(futureTimestamp.Equals(other), Is.False);
            }

            [Test]
            public void With_Same_Value_Returns_True()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.AddMinutes(5);
                var (futureTimestampA, futureTimestampB) = (new FutureTimestamp(rawValue), new FutureTimestamp(rawValue));

                Assert.That(futureTimestampA.Equals(futureTimestampB), Is.True);
            }

            [Test]
            public void With_Different_Values_Returns_False()
            {
                DateTimeOffset rawValueA = DateTimeOffset.UtcNow.AddMinutes(5);
                DateTimeOffset rawValueB = DateTimeOffset.UtcNow.AddMinutes(15);
                var (futureTimestampA, futureTimestampB) 
                    = (new FutureTimestamp(rawValueA), new FutureTimestamp(rawValueB));

                Assert.That(futureTimestampA.Equals(futureTimestampB), Is.False);
            }
        }

        [TestFixture]
        internal sealed class CompareToMessage
        {
            [Test]
            public void With_Null_Returns_Positive()
            {
                (FutureTimestamp futureTimestamp, _) = CreateWithFiveMinutesInTheFuture();

                Assert.That(futureTimestamp.CompareTo(null), Is.GreaterThan(0));
            }

            [Test]
            public void With_Self_Returns_Zero()
            {
                (FutureTimestamp futureTimestamp, _) = CreateWithFiveMinutesInTheFuture();

                Assert.That(futureTimestamp.CompareTo(futureTimestamp), Is.Zero);
            }

            [Test]
            public void Same_As_Raw_Value()
            {
                DateTimeOffset rawValueA = DateTimeOffset.UtcNow.AddDays(5);
                DateTimeOffset rawValueB = DateTimeOffset.UtcNow.AddDays(10);
                var (futureTimestampA, futureTimestampB) = (new FutureTimestamp(rawValueA), new FutureTimestamp(rawValueB));

                Assert.That(futureTimestampA.CompareTo(futureTimestampB), Is.EqualTo(rawValueA.CompareTo(rawValueB)));
            }
        }

        private static (FutureTimestamp futureTimestamp, DateTimeOffset rawValue) CreateWithFiveMinutesInTheFuture()
        {
            DateTimeOffset rawValue = DateTimeOffset.UtcNow.AddMinutes(5);

            return (new FutureTimestamp(rawValue), rawValue);
        }
    }
}
