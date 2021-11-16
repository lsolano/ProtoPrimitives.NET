using Triplex.ProtoDomainPrimitives.Numerics;

namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

[TestFixture]
internal sealed class EqualityComparerFacts
{
    [Test]
    public void GetHashCode_With_Null_Returns_DefaultHashCode()
    {
        PositiveInteger ps = new(1_024);

        Assert.That(ps.GetHashCode(null!), Is.EqualTo(AbstractDomainPrimitive<int>.DefaultHashCode));
    }

    [Test]
    public void GetHashCode_With_NotNull_Returns_Wrapped_Value_HashCode()
    {
        PositiveInteger ps = new(1_024);
        PositiveInteger ps2 = new(2_048);

        Assert.That(ps.GetHashCode(ps2), Is.EqualTo(ps2.GetHashCode()));
    }

    [Test]
    public void Equals_With_Both_Null_Returns_True()
    {
        PositiveInteger ps = new(1_024);

        Assert.That(ps.Equals(null, null), Is.True);
    }

    [Test]
    public void Equals_Some_Null_Returns_False([Values] bool rightIsNull)
    {
        PositiveInteger ps = new(1_024);
        PositiveInteger left = rightIsNull ? new PositiveInteger(2_048) : null!;
        PositiveInteger right = rightIsNull ? null! : new PositiveInteger(2_048);

        Assert.That(ps.Equals(left, right), Is.False);
    }

    [Test]
    public void Equals_Both_Not_Null_Returns_Expected([Values] bool sameValue)
    {
        PositiveInteger ps = new(1_024);
        PositiveInteger left = new(2_048);
        PositiveInteger right = sameValue ? new PositiveInteger(2_048) : new PositiveInteger(4_096);

        Assert.That(ps.Equals(left, right), Is.EqualTo(sameValue));
    }
}
