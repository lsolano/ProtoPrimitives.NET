using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Strings;
using System;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts
{
    [TestFixture]
    internal sealed class BuilderConstructorMessage
    {
        private const string FirstParameterName = "argumentNullErrorMessage";
        private static readonly Message ArgNullErrorMessage = new Message("Some caller provided not null message");

        [Test]
        public void With_Null_ArgumentNullErrorMessage_Throws_Nothing([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            Func<ConfigurableString.Builder> testDelegate = () => useSingleParamConstructor ? new ConfigurableString.Builder(null) : new ConfigurableString.Builder(null, useSingleMessage);

            Assert.That(testDelegate, Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo(FirstParameterName));
        }

        [Test]
        public void With_Valid_Parameters_Throws_Nothing([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            Func<ConfigurableString.Builder> testDelegate =
                () => useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);

            Assert.That(testDelegate, Throws.Nothing);
        }
    }
}
