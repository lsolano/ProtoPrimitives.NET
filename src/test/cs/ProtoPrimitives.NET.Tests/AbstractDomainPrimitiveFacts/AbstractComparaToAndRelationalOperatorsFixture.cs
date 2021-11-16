namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

[TestFixture]
internal abstract class AbstractComparaToAndRelationalOperatorsFixture<TDomainPrimitive, TRawType>
    where TDomainPrimitive : IComparable<TDomainPrimitive>, IEquatable<TDomainPrimitive>
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
            LessThanSubject = lessThanSubject; //Arguments.IsNotDefault(lessThanSubject, nameof(subject));
            Subject = subject; //Arguments.IsNotDefault(subject, nameof(subject));
            CopyOfSubject = copyOfSubject; //Arguments.IsNotDefault(copyOfSubject, nameof(copyOfSubject));
            GreaterThanSubject = greaterThanSubject; //Arguments.IsNotDefault(greaterThanSubject, nameof(greaterThanSubject));
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
        => Assert.That(ExecuteCompareTo(_context.Subject, default), Is.GreaterThan(0));

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
    public void Equals_Returns_True_When_Both_Are_Default()
        => Assert.That(ExecuteEqualsOperator(default, default), Is.True);

    [Test]
    public void Equals_Returns_False_When_Some_Is_Default([Values] bool leftIsDefault)
    {
        TDomainPrimitive? left = leftIsDefault ? default : _context.Subject;
        TDomainPrimitive? right = leftIsDefault ? _context.Subject : default;

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
    public void NotEquals_Returns_False_When_Both_Are_Default()
        => Assert.That(ExecuteNotEqualsOperator(default, default), Is.False);

    [Test]
    public void NotEquals_Returns_True_When_Some_Is_Null([Values] bool leftIsNull)
    {
        TDomainPrimitive? left = leftIsNull ? default : _context.Subject;
        TDomainPrimitive? right = leftIsNull ? _context.Subject : default;

        Assert.That(ExecuteNotEqualsOperator(left, right), Is.True);
    }

    [Test]
    public void NotEquals_Returns_True_When_Are_Different([Values] bool rightIsLessThanLeft)
    {
        TDomainPrimitive? right = rightIsLessThanLeft ? _context.LessThanSubject : _context.GreaterThanSubject;

        Assert.That(ExecuteNotEqualsOperator(_context.Subject, right), Is.True);
    }

    [Test]
    public void NotEquals_Returns_False_When_Both_Have_Same_Value()
        => Assert.That(ExecuteNotEqualsOperator(_context.Subject, _context.CopyOfSubject), Is.False);

    [Test]
    public void NotEquals_Returns_False_When_Same_Instance()
        => Assert.That(ExecuteNotEqualsOperator(_context.Subject, _context.Subject), Is.False);

    #endregion //Not-Equals

    #region LessThan
    [Test]
    public void LessThan_Returns_False_For_Lesser_Than_Subject()
        => Assert.That(ExecuteLessThanOperator(_context.Subject, _context.LessThanSubject), Is.False);

    [Test]
    public void LessThan_Returns_False_For_Subject_Copy()
        => Assert.That(ExecuteLessThanOperator(_context.Subject, _context.CopyOfSubject), Is.False);

    [Test]
    public void LessThan_Returns_False_For_Same_As_Subject()
        => Assert.That(ExecuteLessThanOperator(_context.Subject, _context.Subject), Is.False);

    [Test]
    public void LessThan_Returns_True_For_Greater_Than_As_Subject()
        => Assert.That(ExecuteLessThanOperator(_context.Subject, _context.GreaterThanSubject), Is.True);

    [Test]
    public void LessThan_Returns_False_For_Null_Right()
        => Assert.That(ExecuteLessThanOperator(_context.Subject, default), Is.False);

    [Test]
    public void LessThan_Returns_True_For_Null_Left()
        => Assert.That(ExecuteLessThanOperator(default, _context.Subject), Is.True);

    #endregion //LessThan

    #region LessThanOrEqualsTo
    [Test]
    public void LessThanOrEqualsTo_Returns_False_For_Lesser_Than_Subject()
        => Assert.That(ExecuteLessThanOrEqualsToOperator(_context.Subject, _context.LessThanSubject), Is.False);

    [Test]
    public void LessThanOrEqualsTo_Returns_True_For_Subject_Copy()
        => Assert.That(ExecuteLessThanOrEqualsToOperator(_context.Subject, _context.CopyOfSubject), Is.True);

    [Test]
    public void LessThanOrEqualsTo_Returns_True_For_Same_As_Subject()
        => Assert.That(ExecuteLessThanOrEqualsToOperator(_context.Subject, _context.Subject), Is.True);

    [Test]
    public void LessThanOrEqualsTo_Returns_True_For_Greater_Than_As_Subject()
        => Assert.That(ExecuteLessThanOrEqualsToOperator(_context.Subject, _context.GreaterThanSubject), Is.True);

    [Test]
    public void LessThanOrEqualsTo_Returns_False_For_Null_Right()
        => Assert.That(ExecuteLessThanOrEqualsToOperator(_context.Subject, default), Is.False);

    [Test]
    public void LessThanOrEqualsTo_Returns_True_For_Null_Left()
        => Assert.That(ExecuteLessThanOrEqualsToOperator(default, _context.Subject), Is.True);

    #endregion //LessThanOrEqualsTo

    #region GreaterThan
    [Test]
    public void GreaterThan_Returns_True_For_Lesser_Than_Subject()
        => Assert.That(ExecuteGreaterThanOperator(_context.Subject, _context.LessThanSubject), Is.True);

    [Test]
    public void GreaterThan_Returns_False_For_Subject_Copy()
        => Assert.That(ExecuteGreaterThanOperator(_context.Subject, _context.CopyOfSubject), Is.False);

    [Test]
    public void GreaterThan_Returns_False_For_Same_As_Subject()
        => Assert.That(ExecuteGreaterThanOperator(_context.Subject, _context.Subject), Is.False);

    [Test]
    public void GreaterThan_Returns_False_For_Greater_Than_As_Subject()
        => Assert.That(ExecuteGreaterThanOperator(_context.Subject, _context.GreaterThanSubject), Is.False);

    [Test]
    public void GreaterThan_Returns_True_For_Null_Right()
        => Assert.That(ExecuteGreaterThanOperator(_context.Subject, default), Is.True);

    [Test]
    public void GreaterThan_Returns_False_For_Null_Left()
        => Assert.That(ExecuteGreaterThanOperator(default, _context.Subject), Is.False);

    #endregion //GreaterThan

    #region GreaterThanOrEqualsTo
    [Test]
    public void GreaterThanOrEqualsTo_Returns_True_For_Lesser_Than_Subject()
        => Assert.That(ExecuteGreaterThanOrEqualsToOperator(_context.Subject, _context.LessThanSubject), Is.True);

    [Test]
    public void GreaterThanOrEqualsTo_Returns_True_For_Subject_Copy()
        => Assert.That(ExecuteGreaterThanOrEqualsToOperator(_context.Subject, _context.CopyOfSubject), Is.True);

    [Test]
    public void GreaterThanOrEqualsTo_Returns_True_For_Same_As_Subject()
        => Assert.That(ExecuteGreaterThanOrEqualsToOperator(_context.Subject, _context.Subject), Is.True);

    [Test]
    public void GreaterThanOrEqualsTo_Returns_False_For_Greater_Than_As_Subject()
        => Assert.That(ExecuteGreaterThanOrEqualsToOperator(_context.Subject, _context.GreaterThanSubject),
            Is.False);

    [Test]
    public void GreaterThanOrEqualsTo_Returns_True_For_Null_Right()
        => Assert.That(ExecuteGreaterThanOrEqualsToOperator(_context.Subject, default), Is.True);

    [Test]
    public void GreaterThanOrEqualsTo_Returns_False_For_Null_Left()
        => Assert.That(ExecuteGreaterThanOrEqualsToOperator(default, _context.Subject), Is.False);

    #endregion //GreaterThanOrEqualsTo
}
