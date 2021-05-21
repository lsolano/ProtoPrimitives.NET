using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Temporal;

using static Triplex.ProtoDomainPrimitives.Tests.Temporal.TemporalExtensions;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Temporal
{
    internal static class PastOrPresentTimestampFacts
    {
        [TestFixture]
        internal sealed class ConstructorMessage
        {
            private static readonly IEnumerable<TimeMagnitude> Magnitudes = Enum.GetValues(typeof(TimeMagnitude)).Cast<TimeMagnitude>().ToList();

            private const string ParamName = "rawValue";
            private static readonly Message CustomErrorMessage = new Message("Some custom error message");

            [Test]
            public void Rejects_Future_Values_With_Default_Error_Message(
                [ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, 100);

                Assert.That(() => new PastOrPresentTimestamp(rawValue),
                            BuildArgumentOutOfRangeExceptionConstraint(rawValue, PastOrPresentTimestamp.DefaultErrorMessage));
            }

            [Test]
            public void Rejects_Future_Values_With_Custom_Error_Message(
                [ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, 100);
                

                Assert.That(() => new PastOrPresentTimestamp(rawValue, CustomErrorMessage),
                            BuildArgumentOutOfRangeExceptionConstraint(rawValue, CustomErrorMessage));
            }

            private static IResolveConstraint BuildArgumentOutOfRangeExceptionConstraint(in DateTimeOffset rawValue, in Message errorMessage)
            {
                return Throws.InstanceOf<ArgumentOutOfRangeException>().With.Message.StartsWith(errorMessage.Value)
                                                                       .And.Message.Contains(ParamName)
                                                                       .And.Message.Contains(rawValue.ToString());
            }

            [Test]
            public void Accepts_Present_Value_With_Default_Error_Message()
                => Assert.That(() => new PastOrPresentTimestamp(DateTimeOffset.UtcNow), Throws.Nothing);

            [Test]
            public void Accepts_Present_Value_With_Custom_Error_Message()
                => Assert.That(() => new PastOrPresentTimestamp(DateTimeOffset.UtcNow, CustomErrorMessage), Throws.Nothing);

            [Test]
            public void Accepts_Past_Values_With_Default_Error_Message([ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, -1);

                Assert.That(() => new PastOrPresentTimestamp(rawValue), Throws.Nothing);
            }

            [Test]
            public void Accepts_Past_Values_With_Custom_Error_Message([ValueSource(nameof(Magnitudes))] in TimeMagnitude timeMagnitude)
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow.FromMagnitude(timeMagnitude, -1);

                Assert.That(() => new PastOrPresentTimestamp(rawValue, CustomErrorMessage), Throws.Nothing);
            }
        }

        [TestFixture]
        internal sealed class ValueProperty
        {
            [Test]
            public void Returns_Constructor_Provided_Value()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow;
                var pastOrPresentTimestamp = new PastOrPresentTimestamp(rawValue);

                Assert.That(pastOrPresentTimestamp.Value, Is.EqualTo(rawValue));
            }
        }

        [TestFixture]
        internal sealed class ToStringMessage
        {
            [Test]
            public void Same_As_Raw_Value()
            {
                DateTimeOffset rawValue = DateTimeOffset.UtcNow;
                var pastOrPresentTimestamp = new PastOrPresentTimestamp(rawValue);

                Assert.That(pastOrPresentTimestamp.ToString(), Is.EqualTo(rawValue.ToString()));
            }
        }

        [TestFixture]
        internal sealed class ToISOStringMessage
        {
            [Test]
            public void Formats_String_Using_Full_Date_And_Time_ISO_8601_With_UTC_Z_Indicator()
            {
                int lastYear = DateTimeOffset.UtcNow.Year - 1;
                var rawCreatedTimestamp = new DateTimeOffset(year: lastYear, month: 01, day: 02, hour: 03, minute: 04,
                second: 05, millisecond: 006, offset: TimeSpan.Zero);

                var pastOrPresentTimestamp = new PastOrPresentTimestamp(rawCreatedTimestamp);

                Assert.That(pastOrPresentTimestamp.ToISOString(), Is.EqualTo($"{lastYear}-01-02T03:04:05.006Z"));
            }
        }

        [TestFixture]
        internal sealed class GetHashCodeMessage
        {
            [Test]
            public void Same_As_Raw_Value()
            {
                (PastOrPresentTimestamp pastOrPresentTimestamp, DateTimeOffset rawValue) = CreateFromNow();

                Assert.That(pastOrPresentTimestamp.GetHashCode(), Is.EqualTo(rawValue.GetHashCode()));
            }
        }

        [TestFixture]
        internal sealed class EqualsMessage : IEquatableEqualsFacts<PastOrPresentTimestamp, DateTimeOffset>
        {
            protected override Context CreateContext()
            {
                PastOrPresentTimestamp subject = CreateFromNow().pastOrPresentTimestamp;
                return new IEquatableEqualsFacts<PastOrPresentTimestamp, DateTimeOffset>.Context(
                    subject: subject,
                    subjectValueCopy: new PastOrPresentTimestamp(subject.Value),
                    differentSubject: new PastOrPresentTimestamp(subject.Value.AddMinutes(-15)) 
                );
            }
        }

        [TestFixture]
        internal sealed class CompareToMessage
        {
            [Test]
            public void With_Null_Returns_Positive()
            {
                var pastOrPresentTimestamp = new PastOrPresentTimestamp(DateTimeOffset.UtcNow);

                Assert.That(pastOrPresentTimestamp.CompareTo(null), Is.GreaterThan(0));
            }

            [Test]
            public void With_Self_Returns_Zero()
            {
                var pastOrPresentTimestamp = new PastOrPresentTimestamp(DateTimeOffset.UtcNow);

                Assert.That(pastOrPresentTimestamp.CompareTo(pastOrPresentTimestamp), Is.Zero);
            }

            [Test]
            public void Same_As_Raw_Value()
            {
                DateTimeOffset rawValueA = DateTimeOffset.UtcNow.AddDays(-5);
                DateTimeOffset rawValueB = DateTimeOffset.UtcNow.AddDays(-10);
                var (pastOrPresentTimestampA, pastOrPresentTimestampB) = (new PastOrPresentTimestamp(rawValueA), new PastOrPresentTimestamp(rawValueB));

                Assert.That(pastOrPresentTimestampA.CompareTo(pastOrPresentTimestampB), Is.EqualTo(rawValueA.CompareTo(rawValueB)));
            }
        }

        private static (PastOrPresentTimestamp pastOrPresentTimestamp, DateTimeOffset rawValue) CreateFromNow()
        {
            DateTimeOffset rawValue = DateTimeOffset.UtcNow;

            return (new PastOrPresentTimestamp(rawValue), rawValue);
        }
    }
}
