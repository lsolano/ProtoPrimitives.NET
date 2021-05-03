using System;

using NUnit.Framework;

using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts
{
    [TestFixture]
    internal abstract class IComparableCompareToAndRelationalOperatorsFacts<TDomainPrimitive, TRawType>
        where TDomainPrimitive : AbstractDomainPrimitive<TRawType>
        where TRawType : IComparable<TRawType>, IEquatable<TRawType>
    {
        protected class Context {
            /*
                private readonly PositiveInteger LessThanSubject = new PositiveInteger(9);
                private readonly PositiveInteger Subject = new PositiveInteger(10);
                private readonly PositiveInteger CopyOfSubject = new PositiveInteger(10);
                private readonly PositiveInteger GreaterThanSubject = new PositiveInteger(11);
            */

            public Context(
                in TDomainPrimitive lessThanSubject, 
                in TDomainPrimitive subject, 
                in TDomainPrimitive copyOfSubject,
                in TDomainPrimitive greaterThanSubject)
            {
                LessThanSubject = Arguments.NotNull(lessThanSubject, nameof(lessThanSubject));
                Subject = Arguments.NotNull(subject, nameof(subject));
                CopyOfSubject = Arguments.NotNull(copyOfSubject, nameof(copyOfSubject));
                GreaterThanSubject = Arguments.NotNull(greaterThanSubject, nameof(greaterThanSubject));
            }

            protected internal TDomainPrimitive LessThanSubject { get; }
            protected internal TDomainPrimitive Subject { get; }
            protected internal TDomainPrimitive CopyOfSubject { get; }
            protected internal TDomainPrimitive GreaterThanSubject { get; }
        }

        private readonly Context _context;

        protected IComparableCompareToAndRelationalOperatorsFacts() => _context = CreateContext();

        protected abstract Context CreateContext();

        #region Equals
        [Test]
        public void Equals_Returns_True_When_Both_Are_Null()
            => Assert.That(((TDomainPrimitive)null) == ((TDomainPrimitive)null), Is.True);

        [Test]
        public void Equals_Returns_False_When_Some_Is_Null([Values] bool leftIsNull) {
            var left = leftIsNull? null : _context.Subject;
            var right = leftIsNull? _context.Subject : null;

            Assert.That(left == right, Is.False);
        }

        [Test]
        public void Equals_Returns_False_When_Are_Different([Values] bool rightIsLessThanLeft) {
            var right = rightIsLessThanLeft? _context.LessThanSubject : _context.GreaterThanSubject;

            Assert.That(_context.Subject == right, Is.False);
        }

        [Test]
        public void Equals_Returns_True_When_Both_Have_Same_Value()
            => Assert.That(_context.Subject == _context.CopyOfSubject, Is.True);

        [Test]
        public void Equals_Returns_True_When_Same_Instance()
        #pragma warning disable CS1718 // Comparing same variable
            => Assert.That(_context.Subject == _context.Subject, Is.True);
        #pragma warning restore CS1718 // Comparing same variable

        #endregion //Equals

        #region Not-Equals
        [Test]
        public void NotEquals_Returns_False_When_Both_Are_Null()
            => Assert.That(((TDomainPrimitive)null) != ((TDomainPrimitive)null), Is.False);

        [Test]
        public void NotEquals_Returns_True_When_Some_Is_Null([Values] bool leftIsNull) {
            var left = leftIsNull? null : _context.Subject;
            var right = leftIsNull? _context.Subject : null;

            Assert.That(left != right, Is.True);
        }

        [Test]
        public void NotEquals_Returns_True_When_Are_Different([Values] bool rightIsLessThanLeft) {
            var right = rightIsLessThanLeft? _context.LessThanSubject : _context.GreaterThanSubject;

            Assert.That(_context.Subject != right, Is.True);
        }

        [Test]
        public void NotEquals_Returns_False_When_Both_Have_Same_Value()
            => Assert.That(_context.Subject != _context.CopyOfSubject, Is.False);

        [Test]
        public void NotEquals_Returns_False_When_Same_Instance()
        #pragma warning disable CS1718 // Comparing same variable
            => Assert.That(_context.Subject != _context.Subject, Is.False);
        #pragma warning restore CS1718 // Comparing same variable

        #endregion //Not-Equals


        #region LessThan
        [Test]
        public void LessThan_Returns_False_For_Lesser_Than_Subject()
            => Assert.That(_context.Subject < _context.LessThanSubject, Is.False);

        [Test]
        public void LessThan_Returns_False_For_Same_As_Subject()
            => Assert.That(_context.Subject < _context.CopyOfSubject, Is.False);

        [Test]
        public void LessThan_Returns_True_For_Greater_Than_As_Subject()
            => Assert.That(_context.Subject < _context.GreaterThanSubject, Is.True);

        [Test]
        public void LessThan_Returns_False_For_Null_Right()
            => Assert.That(_context.Subject < null, Is.False);

        [Test]
        public void LessThan_Returns_True_For_Null_Left()
            => Assert.That(null < _context.Subject, Is.True);

        #endregion //LessThan
    }
}