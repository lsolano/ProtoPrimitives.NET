using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.Validations.Exceptions;

namespace Triplex.ProtoDomainPrimitives.Tests.Exceptions.MessageFacts;

[TestFixture]
internal sealed class ConstructorMessageFacts
{
    [Test]
    public void With_Null_Throws_Argument_Null_Exception()
        => Assert.That(() => new Message(null!), Throws.ArgumentNullException);

    [Test]
    public void With_Empty_Throws_ArgumentFormatException()
        => Assert.That(() => new Message(""), Throws.InstanceOf<ArgumentOutOfRangeException>());

    [TestCase(" ")]
    [TestCase(" \t \n \r ")]
    public void With_Semantically_Empty_Throws_ArgumentFormatException(string rawMessage)
        => Assert.That(() => new Message(rawMessage), Throws.InstanceOf<ArgumentFormatException>());

    [TestCase("Error")]
    [TestCase("Some error")]
    [TestCase("Failed \n\r\t check your code \n\r\t see you later ...")]
    public void With_Non_Null_Throws_Nothing(string rawMessage)
        => Assert.That(() => new Message(rawMessage), Throws.Nothing);

    [Test]
    public void Accepts_Real_Exception_Messages_Concatenated_With_Stack_Traces()
    {
        try
        {
            Bar.Compose("Hello", null!);
        }
        catch (Exception ex)
        {
            Assert.That(() => new Message(ex.ToString()), Throws.Nothing);
            Assert.Pass();
        }

        Assert.Fail($"Test design error, an exception was expected as input for {nameof(Message)} constructor.");
    }

    private static class Bar
    {
        internal static string Compose(string baseMessage, string with) => baseMessage + Transform(with);
        internal static string Transform(string with) => GetLengthAsTring(with);
        private static string GetLengthAsTring(string with) => with.Length.ToString();
    }
}
