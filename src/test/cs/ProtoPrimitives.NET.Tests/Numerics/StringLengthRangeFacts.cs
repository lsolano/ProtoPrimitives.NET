using Triplex.ProtoDomainPrimitives.Numerics;

namespace Triplex.ProtoDomainPrimitives.Tests.Numerics;

internal static class StringLengthRangeFacts
{
    private const int DefaultMinRawValue = 100;
    private const int DefaultMaxRawValue = 200;

    private static readonly StringLength DefaultMinLength = new(DefaultMinRawValue);
    private static readonly StringLength DefaultMaxLength = new(DefaultMaxRawValue);

    [TestFixture]
    internal sealed class ConstructorMessage
    {
        [Test]
        public void Accepts_Same_Value_For_Min_And_Max()
            => Assert.That(() => new StringLengthRange(DefaultMinLength, DefaultMinLength), Throws.Nothing);

        [Test]
        public void With_Inverted_Range_Throws_ArgumentOutOfRangeException()
            => Assert.That(() => new StringLengthRange(DefaultMaxLength, DefaultMinLength),
                Throws.InstanceOf<ArgumentOutOfRangeException>());

        [Test]
        public void With_Null_Min_Throws_ArgumentNullException()
            => Assert.That(() => new StringLengthRange(null!, DefaultMaxLength), Throws.ArgumentNullException);

        
        [Test]
        public void With_Null_Max_Throws_ArgumentNullException()
            => Assert.That(() => new StringLengthRange(DefaultMinLength, null!), Throws.ArgumentNullException);
    }

    [TestFixture]
    internal sealed class MinProperty
    {
        [Test]
        public void Returns_Constructor_Provided_Value()
        {
            StringLengthRange ps = new(DefaultMinLength, DefaultMaxLength);

            Assert.That(ps.Min, Is.EqualTo(DefaultMinLength));
        }
    }

    [TestFixture]
    internal sealed class MaxProperty
    {
        [Test]
        public void Returns_Constructor_Provided_Value()
        {
            StringLengthRange ps = new(DefaultMinLength, DefaultMaxLength);

            Assert.That(ps.Max, Is.EqualTo(DefaultMaxLength));
        }
    }
}
