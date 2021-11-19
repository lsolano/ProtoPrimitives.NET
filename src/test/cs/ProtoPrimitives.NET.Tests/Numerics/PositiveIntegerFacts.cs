using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics;

internal static class PositiveIntegerFacts
{
    private const int DefaultRawValue = 1024;
    private static readonly Message CustomErrorMessage = new("Some dummy error message.");

    internal sealed class ConstructorMessage : RawValueAndErrorMessageBaseFixture
    {
        private readonly Message _expectedErrorMessage;

        public ConstructorMessage(bool useCustomMessage) : base(useCustomMessage)
        {
            _expectedErrorMessage = useCustomMessage ? CustomErrorMessage : PositiveInteger.DefaultErrorMessage;
        }

        [Test]
        public void Rejects_Negatives_And_Zero([Values(int.MinValue, -1, 0)] int rawValue)
            => Assert.That(() => Build(rawValue, UseCustomMessage),
                           Throws.InstanceOf<ArgumentOutOfRangeException>()
                                 .With
                                 .Message
                                 .StartsWith(_expectedErrorMessage.Value));

        [Test]
        public void Accepts_Positives([Values(1, DefaultRawValue, int.MaxValue)] int rawValue)
            => Assert.That(() => Build(rawValue, UseCustomMessage), Throws.Nothing);
    }

    [TestFixture]
    internal sealed class ConstructorMessageWithInvalidErrorMessage
    {
        [Test]
        public void Rejects_Null_Custom_Error_Message()
        {
            Assert.That(() => new NegativeInteger(DefaultRawValue, null!),
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
            PositiveInteger ps = new(DefaultRawValue);

            Assert.That(ps.Value, Is.EqualTo(DefaultRawValue));
        }
    }

    [TestFixture]
    internal sealed class ToStringMessage
    {
        [Test]
        public void Same_As_Raw_Value()
        {
            PositiveInteger ps = new(DefaultRawValue);

            Assert.That(ps.ToString(), Is.EqualTo(DefaultRawValue.ToString()));
        }
    }

    [TestFixture]

    internal sealed class GetHashCodeMessage
    {
        [Test]
        public void Same_As_Raw_Value()
        {
            PositiveInteger ps = new(DefaultRawValue);

            Assert.That(ps.GetHashCode(), Is.EqualTo(DefaultRawValue.GetHashCode()));
        }
    }

    [TestFixture]
    internal sealed class EqualsMessage : AbstractEquatableFixture<PositiveInteger, int>
    {
        protected override Context CreateContext()
        {
            return new Context(
                subject: new PositiveInteger(DefaultRawValue, CustomErrorMessage),
                subjectValueCopy: new PositiveInteger(DefaultRawValue, CustomErrorMessage),
                differentSubject: new PositiveInteger(DefaultRawValue * 2, CustomErrorMessage)
            );
        }
    }


    internal sealed class RelationalOperatorsFacts : AbstractCompareToAndRelationalOperatorsFixture<PositiveInteger, int>
    {
        protected override Context CreateContext()
        {
            var subject = new PositiveInteger(10);
            return new Context(
                lessThanSubject: new PositiveInteger(subject.Value - 1),
                subject: subject,
                copyOfSubject: new PositiveInteger(subject.Value),
                greaterThanSubject: new PositiveInteger(subject.Value + 1)
            );
        }

        protected override int ExecuteCompareTo(PositiveInteger self, PositiveInteger? other)
            => self.CompareTo(other);

        protected override bool ExecuteEqualsOperator(PositiveInteger? left, PositiveInteger? right)
            => left! == right!;

        protected override bool ExecuteNotEqualsOperator(PositiveInteger? left, PositiveInteger? right)
            => left! != right!;

        protected override bool ExecuteGreaterThanOperator(PositiveInteger? left, PositiveInteger? right)
            => left! > right!;

        protected override bool ExecuteGreaterThanOrEqualsToOperator(PositiveInteger? left, PositiveInteger? right)
            => left! >= right!;

        protected override bool ExecuteLessThanOperator(PositiveInteger? left, PositiveInteger? right)
            => left! < right!;

        protected override bool ExecuteLessThanOrEqualsToOperator(PositiveInteger? left, PositiveInteger? right)
            => left! <= right!;
    }

    private static PositiveInteger Build(in int rawValue, in bool useCustomMessage)
    {
        return useCustomMessage ? new PositiveInteger(rawValue, CustomErrorMessage) : new PositiveInteger(rawValue);
    }
}
