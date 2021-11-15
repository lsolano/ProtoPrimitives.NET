using NUnit.Framework;
using Triplex.ProtoDomainPrimitives.Strings;
using System;
using static Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace Triplex.ProtoDomainPrimitives.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class BuildWithCustomParserMessage : ValidConstructorArgumentsFixture
    {
        public BuildWithCustomParserMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Null_Rawvalue_Throws_ArgumentNullException()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            const string rawValue = null!;

            Assert.That(() => builder.Build(rawValue, val => { }),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("rawValue")
                    .With.Message.StartsWith(ConstructorMessage.ArgNullErrorMessage.Value));
        }

        [Test]
        public void With_Null_CustomParser_Throws_ArgumentNullException()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            const string rawValue = "Some value using custom parser";

            Assert.That(() => builder.Build(rawValue, null!),
                Throws.ArgumentNullException
                    .With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("customParser"));
        }

        [Test]
        public void Calls_Custom_Parser()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            const string rawValue = "Some value using custom parser";

            bool parserCalled = false;
            ConfigurableString str = builder.Build(rawValue, val => { parserCalled = true; });

            Assert.That(parserCalled, Is.True);
        }

        [Test]
        public void Accepts_Valid_Value()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            const string rawValue = "Some value using custom parser";

            ConfigurableString str = builder.Build(rawValue, val => { });

            Assert.That(str.Value, Is.SameAs(rawValue));
        }

        [Test]
        public void Does_Not_Catch_CustomParser_Exceptions()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);
            const string rawValue = "Some value using custom parser";
            const string customParserExceptionMessage = "I was thrown from custom parser.";

            ConfigurableString testDelegate()
                => builder.Build(rawValue, val => { throw new Exception(customParserExceptionMessage); });

            Assert.That((Func<ConfigurableString>)testDelegate, Throws.Exception
                                                .With.InnerException.Null
                                                .And.Message.EqualTo(customParserExceptionMessage));
        }

        [Test]
        public void Throws_If_Called_Twice()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            _ = builder.Build("Another valid input.");

            Assert.That(() => builder.Build("More good inputs.", val => { }),
                Throws.InstanceOf<InvalidOperationException>()
                    .With.Message.EqualTo(BuildMessage.AlreadyBuiltErrorMessage));
        }
    }
}
