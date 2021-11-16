namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

[TestFixture]
internal abstract class AbstractEquatableFixture<TDomainPrimitive, TRawType>
    where TDomainPrimitive : IComparable<TDomainPrimitive>, IEquatable<TDomainPrimitive>
    where TRawType : IComparable<TRawType>, IEquatable<TRawType>
{
    protected class Context
    {
        public Context(TDomainPrimitive subject, TDomainPrimitive subjectValueCopy,
            TDomainPrimitive differentSubject)
        {
            Subject = subject;
            SubjectValueCopy = subjectValueCopy;
            DifferentSubject = differentSubject;
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
