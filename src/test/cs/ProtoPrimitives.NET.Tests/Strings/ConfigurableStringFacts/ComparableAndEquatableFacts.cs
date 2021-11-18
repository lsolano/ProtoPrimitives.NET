using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;
using Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts;

internal sealed class ComparableAndEquatableFacts
    : AbstractCompareToAndRelationalOperatorsFixture<ConfigurableString, string>
{
    private static readonly Message DummyError = new("Something is not OK");

    protected override Context CreateContext()
    {
        const string rawValue = "abcd";

        ConfigurableString subject = Build(rawValue);

        return new Context(
            lessThanSubject: Build("abcc"),
            subject: subject,
            copyOfSubject: Build(rawValue),
            greaterThanSubject: Build("abce")
        );
    }



    internal static ConfigurableString Build(string rawValue)
    {
        ConfigurableString configurableString = new ConfigurableString.Builder(DummyError, useSingleMessage: true)
            .WithComparisonStrategy(StringComparison.Ordinal)
            .Build(rawValue);

        return configurableString;
    }

    protected override int ExecuteCompareTo(ConfigurableString self, ConfigurableString? other)
        => self.CompareTo(other);

    protected override bool ExecuteEqualsOperator(ConfigurableString? left, ConfigurableString? right)
        => left! == right!;

    protected override bool ExecuteNotEqualsOperator(ConfigurableString? left, ConfigurableString? right)
        => left! != right!;

    protected override bool ExecuteGreaterThanOperator(ConfigurableString? left, ConfigurableString? right)
        => left! > right!;

    protected override bool ExecuteGreaterThanOrEqualsToOperator(ConfigurableString? left, ConfigurableString? right)
        => left! >= right!;

    protected override bool ExecuteLessThanOperator(ConfigurableString? left, ConfigurableString? right)
        => left! < right!;

    protected override bool ExecuteLessThanOrEqualsToOperator(ConfigurableString? left, ConfigurableString? right)
        => left! <= right!;
}
