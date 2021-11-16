using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Temporal;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;
using static Triplex.ProtoDomainPrimitives.Tests.Temporal.TemporalExtensions;

namespace Triplex.ProtoDomainPrimitives.Tests.Temporal;

internal static class FutureTimestampFacts
{
    [TestFixture]
    internal sealed class ConstructorMessage
    {
        private static readonly IEnumerable<TimeMagnitude> Magnitudes = Enum.GetValues(typeof(TimeMagnitude)).Cast<TimeMagnitude>().ToList();

        private const string ParamName = "rawValue";
        private static readonly Message CustomErrorMessage = new("Some custom error message");

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
    internal sealed class ToISOStringMessage
    {
        [Test]
        public void Formats_String_Using_Full_Date_And_Time_ISO_8601_With_UTC_Z_Indicator()
        {
            int lastYear = DateTimeOffset.UtcNow.Year + 1;
            var rawCreatedTimestamp = new DateTimeOffset(year: lastYear, month: 01, day: 02, hour: 03, minute: 04,
            second: 05, millisecond: 006, offset: TimeSpan.Zero);

            var futureTimestamp = new FutureTimestamp(rawCreatedTimestamp);

            Assert.That(futureTimestamp.ToISOString(), Is.EqualTo($"{lastYear}-01-02T03:04:05.006Z"));
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
    internal sealed class EqualsMessage : AbstractEquatableFixture<FutureTimestamp, DateTimeOffset>
    {
        protected override Context CreateContext()
        {
            FutureTimestamp subject = CreateWithFiveMinutesInTheFuture().futureTimestamp;
            return new AbstractEquatableFixture<FutureTimestamp, DateTimeOffset>.Context(
                subject: subject,
                subjectValueCopy: new FutureTimestamp(subject.Value),
                differentSubject: new FutureTimestamp(subject.Value.AddMinutes(15))
            );
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
