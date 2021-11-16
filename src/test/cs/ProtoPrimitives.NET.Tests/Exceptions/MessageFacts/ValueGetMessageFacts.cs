using Triplex.ProtoDomainPrimitives.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Tests.Exceptions.MessageFacts;

[TestFixture]
internal sealed class ValueGetMessageFacts
{
    [TestCase("A")]
    [TestCase(" short ")]
    [TestCase(" list ")]
    [TestCase("of")]
    [TestCase(" error messages.")]
    public void Returns_Constructor_Provided_Message(string rawMessage)
    {
        var message = new Message(rawMessage);

        Assert.That(message.Value, Is.SameAs(rawMessage));
    }
}
