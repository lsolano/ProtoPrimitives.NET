using System;
using NUnit.Framework;
using Triplex.ProtoDomainPrimitives.Exceptions;
using Triplex.ProtoDomainPrimitives.Strings;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class ConstructorMessage : ValidConstructorArgumentsFixture
    {
        private const string FirstParameterName = "argumentNullErrorMessage";
        internal static readonly Message ArgNullErrorMessage = new Message("Some caller provided not null message");

        public ConstructorMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_ArgumentNullErrorMessage_Throws_Nothing()
        {
            Assert.That(() => Create(_useSingleParamConstructor, null, _useSingleMessage),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName)).EqualTo(FirstParameterName));
        }

        [Test]
        public void With_Valid_Parameters_Throws_Nothing()
        => Assert.That(() => Create(_useSingleParamConstructor, ArgNullErrorMessage, _useSingleMessage), Throws.Nothing);

        internal static ConfigurableString.Builder Create(in bool useSingleParamConstructor, in bool useSingleMessage)
            => useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);

        internal static ConfigurableString.Builder Create(in bool useSingleParamConstructor, in Message theMessage, in bool useSingleMessage)
            => useSingleParamConstructor ? new ConfigurableString.Builder(theMessage) : new ConfigurableString.Builder(theMessage, useSingleMessage);
    }
}
