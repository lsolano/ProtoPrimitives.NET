using NUnit.Framework;
using ProtoPrimitives.NET.Exceptions;
using ProtoPrimitives.NET.Strings;
using System;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts
{
    [TestFixture]
    internal sealed class BuilderBuildWithCustomParserMessage
    {
        private static readonly Message ArgNullErrorMessage = new Message("Some caller provided not null message");

        [Test]
        public void With_Null_Rawvalue_Throws_ArgumentNullException([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = null;

            Assert.That(() => builder.Build(rawValue, val => { }),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("rawValue")
                    .With.Message.StartsWith(ArgNullErrorMessage.Value));
        }

        [Test]
        public void With_Null_CustomParser_Throws_ArgumentNullException([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = "Some value using custom parser";

            Assert.That(() => builder.Build(rawValue, null),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("customParser"));
        }

        [Test]
        public void Calls_Custom_Parser([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = "Some value using custom parser";

            bool parserCalled = false;
            ConfigurableString str = builder.Build(rawValue, val => { parserCalled = true; });

            Assert.That(parserCalled, Is.True);
        }

        [Test]
        public void Accepts_Valid_Value([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = "Some value using custom parser";

            ConfigurableString str = builder.Build(rawValue, val => { });

            Assert.That(str.Value, Is.SameAs(rawValue));
        }

        [Test]
        public void Does_Not_Catch_CustomParser_Exceptions([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);
            const string rawValue = "Some value using custom parser";
            const string customParserExceptionMessage = "I was thrown from custom parser.";

            Func<ConfigurableString> testDelegate = () =>  builder.Build(rawValue, val => { throw new Exception(customParserExceptionMessage); });

            Assert.That(testDelegate, Throws.Exception
                                                .With.InnerException.Null
                                                .And.Message.EqualTo(customParserExceptionMessage));
        }

        [Test]
        public void Throws_If_Called_Twice([Values(false, true)] bool useSingleParamConstructor, [Values(false, true)] bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = useSingleParamConstructor ? new ConfigurableString.Builder(ArgNullErrorMessage) : new ConfigurableString.Builder(ArgNullErrorMessage, useSingleMessage);

            _ = builder.Build("Another valid input.");

            Assert.That(() => builder.Build("More good inputs.", val => { }),
                Throws.InstanceOf<InvalidOperationException>()
                    .With.Message.EqualTo("Already built."));
        }
    }
}
