using NUnit.Framework;

/// <summary>
/// Setup fixture
/// </summary>
[SetUpFixture]
[Parallelizable(scope: ParallelScope.All)]
public class OneTimeSetupFixture
{
    /// <summary>
    /// Before any tests
    /// </summary>
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
    }

    /// <summary>
    /// Run after all tests
    /// </summary>
    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
    }
}