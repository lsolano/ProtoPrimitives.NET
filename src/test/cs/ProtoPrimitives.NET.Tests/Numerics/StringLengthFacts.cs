using System;

using NUnit.Framework;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics;

internal static class StringLengthFacts
{
    private const int DefaultRawValue = 1024;
    private static readonly Message CustomErrorMessage = new("Some dummy error message.");

    internal sealed class ConstructorMessage : RawValueAndErrorMessageBaseFixture
    {
        private readonly Message _expectedErrorMessage;

        public ConstructorMessage(bool useCustomMessage) : base(useCustomMessage)
        {
            _expectedErrorMessage = useCustomMessage ? CustomErrorMessage : StringLength.DefaultErrorMessage;
        }

        [Test]
        public void Rejects_Negatives_And_Zero([Values(int.MinValue, -1)] int rawValue)
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
            StringLength sl = new(DefaultRawValue);

            Assert.That(sl.Value, Is.EqualTo(DefaultRawValue));
        }
    }

    [TestFixture]
    internal sealed class ToStringMessage
    {
        [Test]
        public void Same_As_Raw_Value()
        {
            StringLength sl = new(DefaultRawValue);

            Assert.That(sl.ToString(), Is.EqualTo(DefaultRawValue.ToString()));
        }
    }

    [TestFixture]

    internal sealed class GetHashCodeMessage
    {
        [Test]
        public void Same_As_Raw_Value()
        {
            StringLength sl = new(DefaultRawValue);

            Assert.That(sl.GetHashCode(), Is.EqualTo(DefaultRawValue.GetHashCode()));
        }
    }

    [TestFixture]
    internal sealed class EqualsMessage : AbstractEquatableFixture<StringLength, int>
    {
        protected override Context CreateContext()
        {
            return new Context(
                subject: new StringLength(DefaultRawValue, CustomErrorMessage),
                subjectValueCopy: new StringLength(DefaultRawValue, CustomErrorMessage),
                differentSubject: new StringLength(DefaultRawValue * 2, CustomErrorMessage)
            );
        }
    }


    internal sealed class RelationalOperatorsFacts : AbstractComparaToAndRelationalOperatorsFixture<StringLength, int>
    {
        protected override Context CreateContext()
        {
            var subject = new StringLength(10);
            return new Context(
                lessThanSubject: new StringLength(subject.Value - 1),
                subject: subject,
                copyOfSubject: new StringLength(subject.Value),
                greaterThanSubject: new StringLength(subject.Value + 1)
            );
        }

        protected override int ExecuteCompareTo(StringLength self, StringLength? other)
            => self.CompareTo(other);

        protected override bool ExecuteEqualsOperator(StringLength? left, StringLength? right)
            => left! == right!;

        protected override bool ExecuteNotEqualsOperator(StringLength? left, StringLength? right)
            => left! != right!;

        protected override bool ExecuteGreaterThanOperator(StringLength? left, StringLength? right)
            => left! > right!;

        protected override bool ExecuteGreaterThanOrEqualsToOperator(StringLength? left, StringLength? right)
            => left! >= right!;

        protected override bool ExecuteLessThanOperator(StringLength? left, StringLength? right)
            => left! < right!;

        protected override bool ExecuteLessThanOrEqualsToOperator(StringLength? left, StringLength? right)
            => left! <= right!;
    }

    private static StringLength Build(int rawValue, bool useCustomMessage)
    {
        return useCustomMessage ? new StringLength(rawValue, CustomErrorMessage) : new StringLength(rawValue);
    }
}
