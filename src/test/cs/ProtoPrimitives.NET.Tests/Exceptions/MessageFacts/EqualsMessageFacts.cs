using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Exceptions.MessageFacts;

internal sealed class EqualsMessageFacts : AbstractEquatableFixture<Message, string>
{
    protected override Context CreateContext()
    {
        const string rawValue = "abcd";

        Message subject = new(rawValue);
        return new Context(
            subject: subject,
            subjectValueCopy: new(rawValue),
            differentSubject: new Message("xyz")
        );
    }
}
