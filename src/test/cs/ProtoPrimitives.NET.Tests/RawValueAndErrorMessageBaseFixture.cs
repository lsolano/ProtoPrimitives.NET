namespace Triplex.ProtoDomainPrimitives.Tests;

[TestFixture(false)]
[TestFixture(true)]
internal abstract class RawValueAndErrorMessageBaseFixture
{
    protected RawValueAndErrorMessageBaseFixture(bool useCustomMessage) => UseCustomMessage = useCustomMessage;

    protected bool UseCustomMessage { get; }
}
