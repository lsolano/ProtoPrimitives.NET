using NUnit.Framework;

using Triplex.ProtoDomainPrimitives.Numerics;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics
{
    internal static class StringLengthRangeFacts
    {
        [TestFixture]
        internal sealed class ConstructorMessage
        {
            [Test]
            public void Accepts_Same_Value_For_Min_And_Max() {
                (StringLength min, StringLength max) = (new StringLength(11), new StringLength(11));
                
                Assert.That(() => new StringLengthRange(min, max), Throws.Nothing);
            }
        }
    }
}
