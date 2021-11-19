using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Exceptions.MessageFacts;

internal sealed class GetHashCodeMessage : AbstractGetHashCodeFixture<Message>
{
    protected override Context CreateContext()
    {
        const string rawValue = "Argument was not a prime number.";

        return new Context(
            subjectFromFirstCategory: new Message(rawValue),
            subjectFromFirstCategoryCopy: new Message(rawValue),
            subjectFromSecondCategory: new Message("Prime numbers not allowed.")
        );
    }
}
