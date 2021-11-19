using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Numerics;
using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

[TestFixture]
internal sealed class EqualsMessage
{
    /* 
        1. PositiveInteger(5).Equals(PositiveInteger(5)): true
        2. PositiveInteger(5).Equals(PositiveInteger(6)): false
        3. PositiveInteger(5).Equals(NegativeInteger(-5)): false
        4. PositiveInteger(5).Equals(null): false
        5. PositiveInteger(5).Equals("Hello World"): false
    */

    [Test]
    public void Same_Types_And_Values_Returns_True()
    {
        AbstractDomainPrimitive<int> a = new PositiveInteger(5);
        object? b = new PositiveInteger(5);

        Assert.That(a.Equals(b), Is.True);
    }

    [Test]
    public void Same_Types_And_Different_Values_Returns_False()
    {
        AbstractDomainPrimitive<int> a = new PositiveInteger(5);
        object? b = new PositiveInteger(6);

        Assert.That(a.Equals(b), Is.False);
    }

    [Test]
    public void Different_Types_Returns_False()
    {
        AbstractDomainPrimitive<int> a = new PositiveInteger(5);
        object? b = new NegativeInteger(-5);

        Assert.That(a.Equals(b), Is.False);
    }

    [TestCase(null)]
    [TestCase("Hello World")]
    [TestCase(5)]
    public void With_Non_Derived_Types_Returns_False(object? b)
    {
        AbstractDomainPrimitive<int> a = new PositiveInteger(5);

        Assert.That(a.Equals(b), Is.False);
    }

    [TestCase(2, 4)]
    [TestCase(4, 4)]
    [TestCase(4, 6)]
    public void With_Proxy_Child_Class_Returns_Inner_Value_Equals(int rawA, int rawB)
    {
        (AbstractDomainPrimitive<int> a, AbstractDomainPrimitive<int> b) = (new Foo(rawA), new Foo(rawB));

        Assert.That(a.Equals(b), Is.EqualTo(rawA.Equals(rawB)));
    }

    [TestCase(null)]
    [TestCase("Hello World")]
    [TestCase(5)]
    public void With_Proxy_Child_Class_And_Non_Derived_Types_Returns_False(object? b)
    {
        AbstractDomainPrimitive<int> a = new Foo(10);

        Assert.That(a.Equals(b), Is.False);
    }

    private sealed class Foo : AbstractDomainPrimitive<int>
    {
        private static readonly Message NotEvenErrorMessage = new("Only even numbers are accepted.");

        public Foo(int rawValue)
            : base(rawValue, NotEvenErrorMessage, (val, msg) => Validate(val, msg.Value))
        {

        }

        private static int Validate(int rawValue, string errorMessage)
            => (rawValue % 2 == 0) ? rawValue : throw new ArgumentException(errorMessage, nameof(rawValue));

        public override bool Equals(object? obj) => base.Equals(obj);

        public override bool Equals(AbstractDomainPrimitive<int>? other) => base.Equals(other);

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => base.ToString();
    }
}
