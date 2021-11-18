using Triplex.ProtoDomainPrimitives.Strings;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts;

internal sealed class GetHashCodeMessage : AbstractGetHashCodeFixture<ConfigurableString>
{
    protected override Context CreateContext()
    {
        const string rawValue = "Clark Kent";

        return new Context(
            subjectFromFirstCategory: ComparableAndEquatableFacts.Build(rawValue),
            subjectFromFirstCategoryCopy: ComparableAndEquatableFacts.Build(rawValue),
            subjectFromSecondCategory: ComparableAndEquatableFacts.Build("Superman")
        );
    }
}
