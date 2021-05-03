using System;
using NUnit.Framework;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics
{
    internal static class NegativeIntegerFacts
    {
        private const int DefaultRawValue = -1024;
        private static  readonly Message CustomErrorMessage = new Message("Some dummy error message.");

        internal sealed class ConstructorMessage : RawValueAndErrorMessageBaseFixture
        {
            private readonly Message _expectedErrorMessage;

            public ConstructorMessage(bool useCustomMessage) : base(useCustomMessage)
            {
                _expectedErrorMessage = useCustomMessage ? CustomErrorMessage : NegativeInteger.DefaultErrorMessage;
            }

            [Test]
            public void Rejects_Positives_And_Zero([Values(int.MaxValue, 1, 0)] int rawValue)
                => Assert.That(() => Build(rawValue, UseCustomMessage),
                               Throws.InstanceOf<ArgumentOutOfRangeException>()
                                     .With
                                     .Message
                                     .StartsWith(_expectedErrorMessage.Value));

            [Test]
            public void Accepts_Negatives([Values(-1, DefaultRawValue, int.MinValue)] int rawValue)
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
                NegativeInteger ns = Build(DefaultRawValue, UseCustomMessage);

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
                NegativeInteger ns = Build(DefaultRawValue, UseCustomMessage);

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
                NegativeInteger ns = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ns.GetHashCode(), Is.EqualTo(DefaultRawValue.GetHashCode()));
            }
        }

        [TestFixture(false)]
        [TestFixture(true)]
        internal sealed class EqualsMessage : IEquatableEqualsFacts<NegativeInteger, int>, IOptionalCustomMessageTestFixture
        {
            public bool UseCustomMessage { get; }

            protected override Context CreateContext()
            {
                return new IEquatableEqualsFacts<NegativeInteger, int>.Context(
                    subject: UseCustomMessage ? new NegativeInteger(DefaultRawValue, CustomErrorMessage) : new NegativeInteger(DefaultRawValue),
                    subjectValueCopy: UseCustomMessage ? new NegativeInteger(DefaultRawValue, CustomErrorMessage) : new NegativeInteger(DefaultRawValue),
                    differentSubject: UseCustomMessage ? new NegativeInteger(DefaultRawValue * 2, CustomErrorMessage) : new NegativeInteger(DefaultRawValue * 2) 
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
                NegativeInteger ns = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ns.CompareTo(null), Is.GreaterThan(0));
            }

            [Test]
            public void With_Self_Returns_Zero()
            {
                NegativeInteger ns = Build(DefaultRawValue, UseCustomMessage);

                Assert.That(ns.CompareTo(ns), Is.Zero);
            }

            [Test]
            public void Same_As_Raw_Value([Values(-1, -2)] in int rawValueA, [Values(-1, -2)] in int rawValueB)
            {
                (NegativeInteger positiveIntA, NegativeInteger positiveIntB)
                    = (Build(rawValueA, UseCustomMessage), Build(rawValueB, UseCustomMessage));

                Assert.That(positiveIntA.CompareTo(positiveIntB), Is.EqualTo(rawValueA.CompareTo(rawValueB)));
            }
        }

        internal sealed class RelationalOperatorsFacts : IComparableCompareToAndRelationalOperatorsFacts<NegativeInteger, int>
        {
            protected override Context CreateContext()
            {
                var subject = new NegativeInteger(-10);
                return new IComparableCompareToAndRelationalOperatorsFacts<NegativeInteger, int>.Context(
                    lessThanSubject: new NegativeInteger(subject.Value - 1),
                    subject: subject,
                    copyOfSubject: new NegativeInteger(subject.Value),
                    greaterThanSubject: new NegativeInteger(subject.Value + 1)
                );
            }
        }

        private static NegativeInteger Build(in int rawValue, in bool useCustomMessage)
        {
            return useCustomMessage ? new NegativeInteger(rawValue, CustomErrorMessage) : new NegativeInteger(rawValue);
        }
    }
}
