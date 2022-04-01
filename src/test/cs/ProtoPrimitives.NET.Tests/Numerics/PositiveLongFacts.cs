using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics;

internal static class PositiveLongFacts
{
    private const long DefaultRawValue = 1024;
    private static readonly Message CustomErrorMessage = new("Some dummy error message.");

    internal sealed class ConstructorMessage : RawValueAndErrorMessageBaseFixture
    {
        private readonly Message _expectedErrorMessage;

        public ConstructorMessage(bool useCustomMessage) : base(useCustomMessage)
            => _expectedErrorMessage = useCustomMessage ? CustomErrorMessage : PositiveLong.DefaultErrorMessage;

        [Test]
        public void Rejects_Negatives_And_Zero([Values(int.MinValue, -1, 0)] long rawValue)
            => Assert.That(() => Build(rawValue, UseCustomMessage),
                           Throws.InstanceOf<ArgumentOutOfRangeException>()
                                 .With
                                 .Message
                                 .StartsWith(_expectedErrorMessage.Value));

        [Test]
        public void Accepts_Positives([Values(1, DefaultRawValue, int.MaxValue)] long rawValue)
            => Assert.That(() => Build(rawValue, UseCustomMessage), Throws.Nothing);
    }

    [TestFixture]
    internal sealed class ConstructorMessageWithInvalidErrorMessage
    {
        [Test]
        public void Rejects_Null_Custom_Error_Message()
        {
            Assert.That(() => new PositiveLong(DefaultRawValue, null!),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("errorMessage"));
        }
    }

    [TestFixture]
    internal sealed class ValueProperty
    {
        [Test]
        public void Returns_Constructor_Provided_Value()
        {
            PositiveLong ps = new(DefaultRawValue);

            Assert.That(ps.Value, Is.EqualTo(DefaultRawValue));
        }
    }

    [TestFixture]
    internal sealed class ToStringMessage
    {
        [Test]
        public void Same_As_Raw_Value()
        {
            PositiveLong ps = new(DefaultRawValue);

            Assert.That(ps.ToString(), Is.EqualTo(DefaultRawValue.ToString()));
        }
    }

    [TestFixture]

    internal sealed class GetHashCodeMessage
    {
        [Test]
        public void Same_As_Raw_Value()
        {
            PositiveLong ps = new(DefaultRawValue);

            Assert.That(ps.GetHashCode(), Is.EqualTo(DefaultRawValue.GetHashCode()));
        }
    }

    [TestFixture]
    internal sealed class EqualsMessage : AbstractEquatableFixture<PositiveLong, long>
    {
        protected override Context CreateContext()
        {
            return new Context(
                subject: new PositiveLong(DefaultRawValue, CustomErrorMessage),
                subjectValueCopy: new PositiveLong(DefaultRawValue, CustomErrorMessage),
                differentSubject: new PositiveLong(DefaultRawValue * 2, CustomErrorMessage)
            );
        }
    }


    internal sealed class RelationalOperatorsFacts : AbstractCompareToAndRelationalOperatorsFixture<PositiveLong, long>
    {
        protected override Context CreateContext()
        {
            var subject = new PositiveLong(10);
            return new Context(
                lessThanSubject: new PositiveLong(subject.Value - 1),
                subject: subject,
                copyOfSubject: new PositiveLong(subject.Value),
                greaterThanSubject: new PositiveLong(subject.Value + 1)
            );
        }

        protected override int ExecuteCompareTo(PositiveLong self, PositiveLong? other)
            => self.CompareTo(other);

        protected override bool ExecuteEqualsOperator(PositiveLong? left, PositiveLong? right)
            => left! == right!;

        protected override bool ExecuteNotEqualsOperator(PositiveLong? left, PositiveLong? right)
            => left! != right!;

        protected override bool ExecuteGreaterThanOperator(PositiveLong? left, PositiveLong? right)
            => left! > right!;

        protected override bool ExecuteGreaterThanOrEqualsToOperator(PositiveLong? left, PositiveLong? right)
            => left! >= right!;

        protected override bool ExecuteLessThanOperator(PositiveLong? left, PositiveLong? right)
            => left! < right!;

        protected override bool ExecuteLessThanOrEqualsToOperator(PositiveLong? left, PositiveLong? right)
            => left! <= right!;
    }

    private static PositiveLong Build(long rawValue, bool useCustomMessage)
        => useCustomMessage ? new PositiveLong(rawValue, CustomErrorMessage) : new PositiveLong(rawValue);
}
