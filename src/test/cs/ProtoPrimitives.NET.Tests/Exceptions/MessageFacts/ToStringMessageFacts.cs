using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Tests.Exceptions.MessageFacts;


[TestFixture]
internal sealed class ToStringMessageFacts
{
    [Test]
    public void Same_As_Raw_Value()
    {
        const string rawMessage = "Something was not OK.";
        Message message = new(rawMessage);

        Assert.That(message.ToString(), Is.SameAs(rawMessage));
    }
}
