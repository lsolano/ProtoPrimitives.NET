using Triplex.Validations;

namespace Triplex.ProtoDomainPrimitives.Tests.AbstractDomainPrimitiveFacts;

[TestFixture]
internal abstract class AbstractGetHashCodeFixture<TCategorizable> where TCategorizable : class
{
    protected class Context
    {
        public Context(TCategorizable subjectFromFirstCategory, TCategorizable subjectFromFirstCategoryCopy,
            TCategorizable subjectFromSecondCategory)
        {
            SubjectFromFirstCategory = Arguments.NotNull(subjectFromFirstCategory, nameof(subjectFromFirstCategory));
            SubjectFromFirstCategoryCopy =
                Arguments.NotNull(subjectFromFirstCategoryCopy, nameof(subjectFromFirstCategoryCopy));
            SubjectFromSecondCategory =
                Arguments.NotNull(subjectFromSecondCategory, nameof(subjectFromSecondCategory));
        }

        protected internal TCategorizable SubjectFromFirstCategory { get; }
        protected internal TCategorizable SubjectFromFirstCategoryCopy { get; }
        protected internal TCategorizable SubjectFromSecondCategory { get; }
    }

    private readonly Context _context;

    protected AbstractGetHashCodeFixture() => _context = CreateContext();

    protected abstract Context CreateContext();

    [Test]
    public void Both_From_Same_Category_Returns_Same_Hash()
        => Assert.That(_context.SubjectFromFirstCategory.GetHashCode(),
                       Is.EqualTo(_context.SubjectFromFirstCategoryCopy.GetHashCode()));

    [Test]
    public void Instances_From_Different_Categories_Yield_Different_Hashes()
        => Assert.That(_context.SubjectFromFirstCategory.GetHashCode(),
                       Is.Not.EqualTo(_context.SubjectFromSecondCategory.GetHashCode()));
}
