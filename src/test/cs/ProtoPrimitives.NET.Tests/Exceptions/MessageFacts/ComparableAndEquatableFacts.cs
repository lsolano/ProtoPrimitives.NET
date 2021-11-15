using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Exceptions.MessageFacts;

internal sealed class ComparableAndEquatableFacts : AbstractComparaToAndRelationalOperatorsFixture<Message, string>
{
    protected override Context CreateContext()
    {
        const string rawValue = "abcd";

        Message subject = new(rawValue);
        return new Context(
            lessThanSubject: new Message("abcc"),
            subject: subject,
            copyOfSubject: new(rawValue),
            greaterThanSubject: new Message("abce")
        );
    }

    protected override int ExecuteCompareTo(Message self, Message? other)
        => self.CompareTo(other);

    protected override bool ExecuteEqualsOperator(Message? left, Message? right)
        => left! == right!;

    protected override bool ExecuteNotEqualsOperator(Message? left, Message? right)
        => left! != right!;

    protected override bool ExecuteGreaterThanOperator(Message? left, Message? right)
        => left! > right!;

    protected override bool ExecuteGreaterThanOrEqualsToOperator(Message? left, Message? right)
        => left! >= right!;

    protected override bool ExecuteLessThanOperator(Message? left, Message? right)
        => left! < right!;

    protected override bool ExecuteLessThanOrEqualsToOperator(Message? left, Message? right)
        => left! <= right!;
}
