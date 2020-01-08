using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Strings;
using System;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    [TestFixture]
    internal sealed class ConstructorMessage
    {
        private const string FirstParameterName = "argumentNullErrorMessage";
        internal static readonly Message ArgNullErrorMessage = new Message("Some caller provided not null message");

        [Test]
        public void With_Null_ArgumentNullErrorMessage_Throws_Nothing([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            Assert.That(() => Create(useSingleParamConstructor, null, useSingleMessage),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo(FirstParameterName));
        }

        [Test]
        public void With_Valid_Parameters_Throws_Nothing([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            Assert.That(() => Create(useSingleParamConstructor, ArgNullErrorMessage, useSingleMessage), Throws.Nothing);
        }

        internal static ConfigurableString.Builder Create(in bool useSingleParamConstructor, in Message theMessage, in bool useSingleMessage)
            => useSingleParamConstructor ? new ConfigurableString.Builder(theMessage) : new ConfigurableString.Builder(theMessage, useSingleMessage);
    }
}
