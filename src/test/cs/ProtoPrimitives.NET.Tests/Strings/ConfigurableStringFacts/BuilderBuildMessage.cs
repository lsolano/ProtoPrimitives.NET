using System;
using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Strings;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts
{
    [TestFixture]
    internal sealed class BuilderBuildMessage
    {
        private static readonly Message ArgNullErrorMessage = new Message("Some caller provided not null message");

        [Test]
        public void With_Null_Rawvalue_Throws_ArgumentNullException([Values(false, true)] in bool useSingleParamConstructor, [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = null;

            Assert.That(() => builder.Build(rawValue),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("rawValue")
                    .With.Message.StartsWith(ArgNullErrorMessage.Value));
        }

        [Test]
        public void Allows_Build_After_Instantiation([Values(false, true)] in bool useSingleParamConstructor, [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = "I'm a valid string";

            ConfigurableString str = builder.Build(rawValue);

            Assert.That(str.Value, Is.SameAs(rawValue));
        }

        [Test]
        public void Throws_If_Called_Twice([Values(false, true)] in bool useSingleParamConstructor, [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);

            _ = builder.Build("I'm a valid string");

            Assert.That(() => builder.Build("Some other value."), Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo("Already built."));
        }
    }
}