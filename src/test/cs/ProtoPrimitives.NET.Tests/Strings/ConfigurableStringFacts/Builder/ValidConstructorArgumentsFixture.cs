using NUnit.Framework;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    [TestFixture(false, false)]
    [TestFixture(false, true)]
    [TestFixture(true, false)]
    internal abstract class ValidConstructorArgumentsFixture
    {
        protected internal readonly bool _useSingleParamConstructor;
        protected internal readonly bool _useSingleMessage;

        protected ValidConstructorArgumentsFixture(
            bool useSingleParamConstructor,
            bool useSingleMessage)
        {
            _useSingleParamConstructor = useSingleParamConstructor;
            _useSingleMessage = useSingleMessage;
        }
    }
}