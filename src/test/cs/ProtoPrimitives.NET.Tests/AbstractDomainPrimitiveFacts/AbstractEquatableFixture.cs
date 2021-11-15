using System;

using NUnit.Framework;

using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts
{
    [TestFixture]
    internal abstract class AbstractEquatableFixture<TDomainPrimitive, TRawType>
        where TDomainPrimitive : AbstractDomainPrimitive<TRawType>
        where TRawType : IComparable<TRawType>, IEquatable<TRawType>
    {
        protected class Context {
            public Context(
                in TDomainPrimitive subject, 
                in TDomainPrimitive subjectValueCopy, 
                in TDomainPrimitive differentSubject)
            {
                Subject = Arguments.NotNull(subject, nameof(subject));
                SubjectValueCopy = Arguments.NotNull(subjectValueCopy, nameof(subjectValueCopy));
                DifferentSubject = Arguments.NotNull(differentSubject, nameof(differentSubject));
            }

            protected internal TDomainPrimitive Subject { get; }
            protected internal TDomainPrimitive SubjectValueCopy { get; }
            protected internal TDomainPrimitive DifferentSubject { get; }
        }

        private readonly Context _context;

        protected AbstractEquatableFixture() => _context = CreateContext();

        protected abstract Context CreateContext();

        [TestCase(null)]
        [TestCase("Hello World")]
        public void With_Null_And_Other_Type_Returns_False(object other)
            => Assert.That(_context.Subject.Equals(other), Is.False);

        [Test]
        public void With_Self_Returns_True()
            => Assert.That(_context.Subject.Equals(_context.Subject), Is.True);

        [Test]
        public void With_Value_Copy_Returns_True()
            => Assert.That(_context.Subject.Equals(_context.SubjectValueCopy), Is.True);

        [Test]
        public void With_Different_Value_Returns_False()
            => Assert.That(_context.Subject.Equals(_context.DifferentSubject), Is.False);
    }
}
