using System;

using NUnit.Framework;

using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics
{
    internal static class PositiveIntegerFacts
    {
        private const int DefaultRawValue = 1024;
        private static  readonly Message CustomErrorMessage = new Message("Some dummy error message.");

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
                Assert.That(() => new NegativeInteger(DefaultRawValue, null),
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
                PositiveInteger ps = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ps.Value, Is.EqualTo(DefaultRawValue));
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
                PositiveInteger ps = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ps.ToString(), Is.EqualTo(DefaultRawValue.ToString()));
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
                PositiveInteger ps = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ps.GetHashCode(), Is.EqualTo(DefaultRawValue.GetHashCode()));
            }
        }

        [TestFixture(false)]
        [TestFixture(true)]
        internal sealed class EqualsMessage : IEquatableEqualsFacts<PositiveInteger, int>, IOptionalCustomMessageTestFixture
        {
            public bool UseCustomMessage { get; }

            protected override Context CreateContext()
            {
                return new IEquatableEqualsFacts<PositiveInteger, int>.Context(
                    subject: UseCustomMessage ? new PositiveInteger(DefaultRawValue, CustomErrorMessage) : new PositiveInteger(DefaultRawValue),
                    subjectValueCopy: UseCustomMessage ? new PositiveInteger(DefaultRawValue, CustomErrorMessage) : new PositiveInteger(DefaultRawValue),
                    differentSubject: UseCustomMessage ? new PositiveInteger(DefaultRawValue * 2, CustomErrorMessage) : new PositiveInteger(DefaultRawValue * 2) 
                );
            }

            public EqualsMessage(bool useCustomMessage) => UseCustomMessage = useCustomMessage;
        }

        internal sealed class CompareToMessage : RawValueAndErrorMessageBaseFixture
        {
            public CompareToMessage(bool useCustomMessage) : base(useCustomMessage)
            {
            }

            [Test]
            public void With_Null_Returns_Positive()
            {
                PositiveInteger ps = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ps.CompareTo(null), Is.GreaterThan(0));
            }

            [Test]
            public void With_Self_Returns_Zero()
            {
                PositiveInteger ps = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ps.CompareTo(ps), Is.Zero);
            }

            [Test]
            public void Same_As_Raw_Value([Values(1, 2)] int rawValueA, [Values(1, 2)] int rawValueB)
            {
                (PositiveInteger positiveIntA, PositiveInteger positiveIntB)
                    = (Build(rawValueA, UseCustomMessage), Build(rawValueB, UseCustomMessage));

                Assert.That(positiveIntA.CompareTo(positiveIntB), Is.EqualTo(rawValueA.CompareTo(rawValueB)));
            }
        }

        internal sealed class RelationalOperatorsFacts : IComparableCompareToAndRelationalOperatorsFacts<PositiveInteger, int>
        {
            protected override Context CreateContext()
            {
                var subject = new PositiveInteger(10);
                return new IComparableCompareToAndRelationalOperatorsFacts<PositiveInteger, int>.Context(
                    lessThanSubject: new PositiveInteger(subject.Value - 1),
                    subject: subject,
                    copyOfSubject: new PositiveInteger(subject.Value),
                    greaterThanSubject: new PositiveInteger(subject.Value + 1)
                );
            }
        }

        private static PositiveInteger Build(in int rawValue, in bool useCustomMessage)
        {
            return useCustomMessage ? new PositiveInteger(rawValue, CustomErrorMessage) : new PositiveInteger(rawValue);
        }
    }
}
