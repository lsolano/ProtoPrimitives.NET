using System;

using NUnit.Framework;

using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts
{
    [TestFixture]
    internal abstract class AbstractComparaToAndRelationalOperatorsFixture<TDomainPrimitive, TRawType>
        where TDomainPrimitive : AbstractDomainPrimitive<TRawType>
        where TRawType : IComparable<TRawType>, IEquatable<TRawType>
    {
        protected class Context
        {
            public Context(
                TDomainPrimitive lessThanSubject,
                TDomainPrimitive subject,
                TDomainPrimitive copyOfSubject,
                TDomainPrimitive greaterThanSubject)
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

        protected AbstractComparaToAndRelationalOperatorsFixture() => _context = CreateContext();

        protected abstract Context CreateContext();

        protected abstract bool ExecuteEqualsOperator(TDomainPrimitive? left, TDomainPrimitive? right);
        protected abstract bool ExecuteGreaterThanOperator(TDomainPrimitive? left, TDomainPrimitive? right);
        protected abstract bool ExecuteGreaterThanOrEqualsToOperator(TDomainPrimitive? left, TDomainPrimitive? right);
        protected abstract bool ExecuteLessThanOperator(TDomainPrimitive? left, TDomainPrimitive? right);
        protected abstract bool ExecuteLessThanOrEqualsToOperator(TDomainPrimitive? left, TDomainPrimitive? right);
        protected abstract bool ExecuteNotEqualsOperator(TDomainPrimitive? left, TDomainPrimitive? right);
        protected abstract int ExecuteCompareTo(TDomainPrimitive self, TDomainPrimitive? other);

        #region CompareTo

        [Test]
        public void With_Null_Returns_Positive()
            => Assert.That(ExecuteCompareTo(_context.Subject, null), Is.GreaterThan(0));

        [Test]
        public void With_Self_Returns_Zero()
            => Assert.That(ExecuteCompareTo(_context.Subject, _context.Subject), Is.Zero);

        [Test]
        public void With_Subject_Copy_Returns_Zero()
            => Assert.That(ExecuteCompareTo(_context.Subject, _context.CopyOfSubject), Is.Zero);

        [Test]
        public void With_Less_Than_Subject_Returns_Positive()
            => Assert.That(ExecuteCompareTo(_context.Subject, _context.LessThanSubject), Is.Positive);

        [Test]
        public void With_Greater_Than_Subject_Returns_Negative()
            => Assert.That(ExecuteCompareTo(_context.Subject, _context.GreaterThanSubject), Is.Negative);

        #endregion //CompareTo

        #region Equals
        [Test]
        public void Equals_Returns_True_When_Both_Are_Null()
            => Assert.That(ExecuteEqualsOperator(null, null), Is.True);

        [Test]
        public void Equals_Returns_False_When_Some_Is_Null([Values] bool leftIsNull)
        {
            TDomainPrimitive? left = leftIsNull ? null : _context.Subject;
            TDomainPrimitive? right = leftIsNull ? _context.Subject : null;

            Assert.That(ExecuteEqualsOperator(left, right), Is.False);
        }

        [Test]
        public void Equals_Returns_False_When_Are_Different([Values] bool rightIsLessThanLeft)
        {
            var right = rightIsLessThanLeft ? _context.LessThanSubject : _context.GreaterThanSubject;

            Assert.That(ExecuteEqualsOperator(_context.Subject, right), Is.False);
        }

        [Test]
        public void Equals_Returns_True_When_Both_Have_Same_Value()
            => Assert.That(ExecuteEqualsOperator(_context.Subject, _context.CopyOfSubject), Is.True);

        [Test]
        public void Equals_Returns_True_When_Same_Instance()
            => Assert.That(ExecuteEqualsOperator(_context.Subject, _context.Subject), Is.True);

        #endregion //Equals

        #region Not-Equals
        [Test]
        public void NotEquals_Returns_False_When_Both_Are_Null()
            => Assert.That((TDomainPrimitive)null! != null!, Is.False);

        [Test]
        public void NotEquals_Returns_True_When_Some_Is_Null([Values] bool leftIsNull)
        {
            var left = leftIsNull ? null : _context.Subject;
            var right = leftIsNull ? _context.Subject : null;

            Assert.That(left! != right!, Is.True);
        }

        [Test]
        public void NotEquals_Returns_True_When_Are_Different([Values] bool rightIsLessThanLeft)
        {
            var right = rightIsLessThanLeft ? _context.LessThanSubject : _context.GreaterThanSubject;

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
        public void LessThan_Returns_False_For_Subject_Copy()
            => Assert.That(_context.Subject < _context.CopyOfSubject, Is.False);

        [Test]
        public void LessThan_Returns_False_For_Same_As_Subject()
            => Assert.That(_context.Subject < _context.Subject, Is.False);

        [Test]
        public void LessThan_Returns_True_For_Greater_Than_As_Subject()
            => Assert.That(_context.Subject < _context.GreaterThanSubject, Is.True);

        [Test]
        public void LessThan_Returns_False_For_Null_Right()
            => Assert.That(_context.Subject < null!, Is.False);

        [Test]
        public void LessThan_Returns_True_For_Null_Left()
            => Assert.That(null! < _context.Subject, Is.True);

        #endregion //LessThan

        #region LessThanOrEqualsTo
        [Test]
        public void LessThanOrEqualsTo_Returns_False_For_Lesser_Than_Subject()
            => Assert.That(_context.Subject <= _context.LessThanSubject, Is.False);

        [Test]
        public void LessThanOrEqualsTo_Returns_True_For_Subject_Copy()
            => Assert.That(_context.Subject <= _context.CopyOfSubject, Is.True);

        [Test]
        public void LessThanOrEqualsTo_Returns_True_For_Same_As_Subject()
            => Assert.That(_context.Subject <= _context.Subject, Is.True);

        [Test]
        public void LessThanOrEqualsTo_Returns_True_For_Greater_Than_As_Subject()
            => Assert.That(_context.Subject <= _context.GreaterThanSubject, Is.True);

        [Test]
        public void LessThanOrEqualsTo_Returns_False_For_Null_Right()
            => Assert.That(_context.Subject <= null!, Is.False);

        [Test]
        public void LessThanOrEqualsTo_Returns_True_For_Null_Left()
            => Assert.That(null! <= _context.Subject, Is.True);

        #endregion //LessThanOrEqualsTo

        #region GreaterThan
        [Test]
        public void GreaterThan_Returns_True_For_Lesser_Than_Subject()
            => Assert.That(_context.Subject > _context.LessThanSubject, Is.True);

        [Test]
        public void GreaterThan_Returns_False_For_Subject_Copy()
            => Assert.That(_context.Subject > _context.CopyOfSubject, Is.False);

        [Test]
        public void GreaterThan_Returns_False_For_Same_As_Subject()
            => Assert.That(_context.Subject > _context.Subject, Is.False);

        [Test]
        public void GreaterThan_Returns_False_For_Greater_Than_As_Subject()
            => Assert.That(_context.Subject > _context.GreaterThanSubject, Is.False);

        [Test]
        public void GreaterThan_Returns_True_For_Null_Right()
            => Assert.That(_context.Subject > null!, Is.True);

        [Test]
        public void GreaterThan_Returns_False_For_Null_Left()
            => Assert.That(null! > _context.Subject, Is.False);

        #endregion //GreaterThan

        #region GreaterThanOrEqualsTo
        [Test]
        public void GreaterThanOrEqualsTo_Returns_True_For_Lesser_Than_Subject()
            => Assert.That(_context.Subject >= _context.LessThanSubject, Is.True);

        [Test]
        public void GreaterThanOrEqualsTo_Returns_True_For_Subject_Copy()
            => Assert.That(_context.Subject >= _context.CopyOfSubject, Is.True);

        [Test]
        public void GreaterThanOrEqualsTo_Returns_True_For_Same_As_Subject()
            => Assert.That(_context.Subject >= _context.Subject, Is.True);

        [Test]
        public void GreaterThanOrEqualsTo_Returns_False_For_Greater_Than_As_Subject()
            => Assert.That(_context.Subject >= _context.GreaterThanSubject, Is.False);

        [Test]
        public void GreaterThanOrEqualsTo_Returns_True_For_Null_Right()
            => Assert.That(_context.Subject >= null!, Is.True);

        [Test]
        public void GreaterThanOrEqualsTo_Returns_False_For_Null_Left()
            => Assert.That(null! >= _context.Subject, Is.False);

        #endregion //GreaterThanOrEqualsTo
    }
}

/*
#region Assembly Wepsys.Core.Facts, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// Wepsys.Core.Facts.dll
#endregion

using NUnit.Framework;
using System;

namespace Wepsys.Core.Facts
{
    [TestFixture]
    public abstract class AbstractComparableTestFixture<TType> where TType : class, IComparable<TType>
    {
        public AbstractComparableTestFixture();

        [TestCase(SubjectIs.LeftSide, OtherIs.Self, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Copy, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.SemanticallyEquals, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Other, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Less, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Greater, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Self, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Copy, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.SemanticallyEquals, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Other, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Less, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Greater, ExpectedResult = false)]
        public bool EqualsOperator_With_None_Null(SubjectIs subjectIs, OtherIs otherIs);
        [TestCase(BinaryOperatorArgumentsNullability.Both, ExpectedResult = true)]
        [TestCase(BinaryOperatorArgumentsNullability.Left, ExpectedResult = false)]
        [TestCase(BinaryOperatorArgumentsNullability.Right, ExpectedResult = false)]
        public bool EqualsOperator_With_Some_Null(BinaryOperatorArgumentsNullability nullability);
        [TestCase(SubjectIs.LeftSide, OtherIs.Self, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Copy, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.SemanticallyEquals, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Less, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Greater, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Self, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Copy, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.SemanticallyEquals, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Less, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Greater, ExpectedResult = true)]
        public bool GreaterThanOrEqualsTo_Operator_With_None_Null(SubjectIs subjectIs, OtherIs otherIs);
        [TestCase(BinaryOperatorArgumentsNullability.Both, ExpectedResult = true)]
        [TestCase(BinaryOperatorArgumentsNullability.Left, ExpectedResult = false)]
        [TestCase(BinaryOperatorArgumentsNullability.Right, ExpectedResult = true)]
        public bool GreaterThanOrEqualsTo_Operator_With_Some_Null(BinaryOperatorArgumentsNullability nullability);
        [TestCase(SubjectIs.LeftSide, OtherIs.Self, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Copy, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.SemanticallyEquals, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Less, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Greater, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Self, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Copy, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.SemanticallyEquals, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Less, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Greater, ExpectedResult = true)]
        public bool GreaterThan_Operator_With_None_Null(SubjectIs subjectIs, OtherIs otherIs);
        [TestCase(BinaryOperatorArgumentsNullability.Both, ExpectedResult = false)]
        [TestCase(BinaryOperatorArgumentsNullability.Left, ExpectedResult = false)]
        [TestCase(BinaryOperatorArgumentsNullability.Right, ExpectedResult = true)]
        public bool GreaterThan_Operator_With_Some_Null(BinaryOperatorArgumentsNullability nullability);
        [TestCase(SubjectIs.LeftSide, OtherIs.Self, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Copy, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.SemanticallyEquals, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Less, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Greater, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Self, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Copy, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.SemanticallyEquals, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Less, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Greater, ExpectedResult = false)]
        public bool LessThanOrEqualsTo_Operator_With_None_Null(SubjectIs subjectIs, OtherIs otherIs);
        [TestCase(BinaryOperatorArgumentsNullability.Both, ExpectedResult = true)]
        [TestCase(BinaryOperatorArgumentsNullability.Left, ExpectedResult = true)]
        [TestCase(BinaryOperatorArgumentsNullability.Right, ExpectedResult = false)]
        public bool LessThanOrEqualsTo_Operator_With_Some_Null(BinaryOperatorArgumentsNullability nullability);
        [TestCase(SubjectIs.LeftSide, OtherIs.Self, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Copy, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.SemanticallyEquals, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Less, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Greater, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Self, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Copy, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.SemanticallyEquals, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Less, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Greater, ExpectedResult = false)]
        public bool LessThan_Operator_With_None_Null(SubjectIs subjectIs, OtherIs otherIs);
        [TestCase(BinaryOperatorArgumentsNullability.Both, ExpectedResult = false)]
        [TestCase(BinaryOperatorArgumentsNullability.Left, ExpectedResult = true)]
        [TestCase(BinaryOperatorArgumentsNullability.Right, ExpectedResult = false)]
        public bool LessThan_Operator_With_Some_Null(BinaryOperatorArgumentsNullability nullability);
        [TestCase(SubjectIs.LeftSide, OtherIs.Self, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Copy, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.SemanticallyEquals, ExpectedResult = false)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Other, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Less, ExpectedResult = true)]
        [TestCase(SubjectIs.LeftSide, OtherIs.Greater, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Self, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Copy, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.SemanticallyEquals, ExpectedResult = false)]
        [TestCase(SubjectIs.RightSide, OtherIs.Other, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Less, ExpectedResult = true)]
        [TestCase(SubjectIs.RightSide, OtherIs.Greater, ExpectedResult = true)]
        public bool NotEqualsOperator_With_None_Null(SubjectIs subjectIs, OtherIs otherIs);
        [TestCase(BinaryOperatorArgumentsNullability.Both, ExpectedResult = false)]
        [TestCase(BinaryOperatorArgumentsNullability.Left, ExpectedResult = true)]
        [TestCase(BinaryOperatorArgumentsNullability.Right, ExpectedResult = true)]
        public bool NotEqualsOperator_With_Some_Null(BinaryOperatorArgumentsNullability nullability);
        [Test]
        public void With_Copy_Returns_Zero();
        [Test]
        public void With_Greater_Returns_Greater_Than_Zero();
        [Test]
        public void With_Lesser_Returns_Greater_Than_Zero();
        [Test]
        public void With_Null_Returns_Greater_Than_Zero();
        [Test]
        public void With_Self_Returns_Zero();
        [Test]
        public void With_Semantically_Equals_Returns_Zero();
        protected abstract Context BuildContext();
        protected virtual bool ExecuteEqualsOperator(in TType left, in TType right);
        protected virtual bool ExecuteGreaterThanOperator(in TType left, in TType right);
        protected virtual bool ExecuteGreaterThanOrEqualsToOperator(in TType left, in TType right);
        protected virtual bool ExecuteLessThanOperator(in TType left, in TType right);
        protected virtual bool ExecuteLessThanOrEqualsToOperator(in TType left, in TType right);
        protected virtual bool ExecuteNotEqualsOperator(in TType left, in TType right);

        protected sealed class Context
        {
            public Context(in TType subject, in TType subjectCopy, in TType otherSemanticallyEqualsToSubject, in TType lessThanSubject, in TType greatherThanSubject);
            public Context(in TType subject, in TType subjectCopy, in TType otherSemanticallyEqualsToSubject, in TType lessThanSubject, in TType greatherThanSubject, in bool testRelationalOperatorsOverload);

            public TType Subject { get; }
            public TType SubjectCopy { get; }
            public TType Other { get; }
            public TType OtherSemanticallyEqualsToSubject { get; }
            public TType LessThanSubject { get; }
            public TType GreatherThanSubject { get; }
            public bool TestRelationalOperatorsOverload { get; }
        }
    }
}
*/
