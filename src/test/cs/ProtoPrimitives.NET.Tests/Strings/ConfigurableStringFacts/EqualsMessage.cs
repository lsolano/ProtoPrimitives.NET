using Triplex.ProtoDomainPrimitives.Strings;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts;

internal sealed class EqualsMessage : AbstractEquatableFixture<ConfigurableString, string>
{
    protected override Context CreateContext()
    {
        const string rawValue = "Peter Parker";

        return new Context(
            subject: ComparableAndEquatableFacts.Build(rawValue),
            subjectValueCopy: ComparableAndEquatableFacts.Build(rawValue),
            differentSubject: ComparableAndEquatableFacts.Build("Spiderman")
        );
    }
}
