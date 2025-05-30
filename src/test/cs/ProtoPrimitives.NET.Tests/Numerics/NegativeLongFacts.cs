using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics;

internal static class NegativeLongFacts
{
    private const long DefaultRawValue = -1024;
    private static readonly Message CustomErrorMessage = new("Some dummy error message.");

    internal sealed class ConstructorMessage : RawValueAndErrorMessageBaseFixture
    {
        private readonly Message _expectedErrorMessage;

        public ConstructorMessage(bool useCustomMessage) : base(useCustomMessage)
            => _expectedErrorMessage = useCustomMessage ? CustomErrorMessage : NegativeLong.DefaultErrorMessage;

        [Test]
        public void Rejects_Positives_And_Zero([Values(long.MaxValue, 1, 0)] long rawValue)
            => Assert.That(() => Build(rawValue, UseCustomMessage),
                           Throws.InstanceOf<ArgumentOutOfRangeException>()
                                 .With
                                 .Message
                                 .StartsWith(_expectedErrorMessage.Value));

        [Test]
        public void Accepts_Negatives([Values(-1, DefaultRawValue, long.MinValue)] long rawValue)
            => Assert.That(() => Build(rawValue, UseCustomMessage), Throws.Nothing);
    }

    [TestFixture]
    internal sealed class ConstructorMessageWithInvalidErrorMessage
    {
        [Test]
        public void Rejects_Null_Custom_Error_Message()
        {
            Assert.That(() => new NegativeLong(DefaultRawValue, null!),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("errorMessage"));
        }
    }

    internal sealed class ValueProperty : RawValueAndErrorMessageBaseFixture
    {
        public ValueProperty(bool useCustomMessage) : base(useCustomMessage)
        {
        }

        [Test]
        public void Returns_Constructor_Provided_Value()
        {
            NegativeLong ns = Build(DefaultRawValue, UseCustomMessage);

            Assert.That(ns.Value, Is.EqualTo(DefaultRawValue));
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
            NegativeLong ns = Build(DefaultRawValue, UseCustomMessage);

            Assert.That(ns.ToString(), Is.EqualTo(DefaultRawValue.ToString()));
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
            NegativeLong ns = Build(DefaultRawValue, UseCustomMessage);

            Assert.That(ns.GetHashCode(), Is.EqualTo(DefaultRawValue.GetHashCode()));
        }
    }

    [TestFixture]
    internal sealed class EqualsMessage : AbstractEquatableFixture<NegativeLong, long>
    {
        protected override Context CreateContext()
        {
            return new Context(
                subject: new NegativeLong(DefaultRawValue, CustomErrorMessage),
                subjectValueCopy: new NegativeLong(DefaultRawValue, CustomErrorMessage),
                differentSubject: new NegativeLong(DefaultRawValue * 2, CustomErrorMessage)
            );
        }
    }

    internal sealed class RelationalOperatorsFacts :
        AbstractCompareToAndRelationalOperatorsFixture<NegativeLong, long>
    {
        protected override Context CreateContext()
        {
            var subject = new NegativeLong(-10);
            return new Context(
                lessThanSubject: new NegativeLong(subject.Value - 1),
                subject: subject,
                copyOfSubject: new NegativeLong(subject.Value),
                greaterThanSubject: new NegativeLong(subject.Value + 1)
            );
        }

        protected override int ExecuteCompareTo(NegativeLong self, NegativeLong? other)
            => self.CompareTo(other);

        protected override bool ExecuteEqualsOperator(NegativeLong? left, NegativeLong? right)
            => left! == right!;

        protected override bool ExecuteNotEqualsOperator(NegativeLong? left, NegativeLong? right)
            => left! != right!;

        protected override bool ExecuteGreaterThanOperator(NegativeLong? left, NegativeLong? right)
            => left! > right!;

        protected override bool ExecuteGreaterThanOrEqualsToOperator(NegativeLong? left, NegativeLong? right)
            => left! >= right!;

        protected override bool ExecuteLessThanOperator(NegativeLong? left, NegativeLong? right)
            => left! < right!;

        protected override bool ExecuteLessThanOrEqualsToOperator(NegativeLong? left, NegativeLong? right)
            => left! <= right!;
    }

    private static NegativeLong Build(long rawValue, bool useCustomMessage)
        => useCustomMessage ? new NegativeLong(rawValue, CustomErrorMessage) : new NegativeLong(rawValue);
}
