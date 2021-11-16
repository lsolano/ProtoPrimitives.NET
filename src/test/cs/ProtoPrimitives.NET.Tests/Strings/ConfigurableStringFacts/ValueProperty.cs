using Triplex.ProtoDomainPrimitives.Strings;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts;

[TestFixture]
internal sealed class ValueProperty
{
    [Test]
    public void Returns_Constructor_Provided_Value()
    {
        const string rawValue = "Peter Parker";
        ConfigurableString cs = ComparableAndEquatableFacts.Build(rawValue);

        Assert.That(cs.Value, Is.SameAs(rawValue));
    }
}
