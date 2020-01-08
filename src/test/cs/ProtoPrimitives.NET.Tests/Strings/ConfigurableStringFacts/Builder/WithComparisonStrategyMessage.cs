using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    internal sealed class WithComparisonStrategyMessage : ValidConstructorArgumentsFixture
    {
        private static readonly IEnumerable<StringComparison> AllStrategies = Enum.GetValues(typeof(StringComparison)).Cast<StringComparison>().ToList();

        public WithComparisonStrategyMessage(bool useSingleParamConstructor, bool useSingleMessage) : base(useSingleParamConstructor, useSingleMessage)
        {
        }

        [Test]
        public void With_Valid_Values_Throws_Nothing([ValueSource(nameof(AllStrategies))] StringComparison strategy)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => builder.WithComparisonStrategy(strategy), Throws.Nothing);
        }

        [Test]
        public void With_Invalid_Values_Throws_ArgumentOutOfRangeException([Values(-1, 6)] StringComparison strategy)
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            Assert.That(() => builder.WithComparisonStrategy(strategy),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("comparisonStrategy")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(strategy));
        }

        [Test]
        public void Can_Be_Called_Twice()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            builder.WithComparisonStrategy(StringComparison.InvariantCulture);

            Assert.That(() => builder.WithComparisonStrategy(StringComparison.Ordinal), Throws.Nothing);
        }

        [Test]
        public void Use_Default_Strategy_For_Comparison()
        {
            string[] rawValues = new[] { "a", "b", "c", "A", "B", "C" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, _useSingleParamConstructor, _useSingleMessage, null);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(actual, Is.EqualTo("ABCabc"));
        }

        [Test]
        public void Use_Default_Strategy_For_Equality()
        {
            string[] rawValues = new[] { "aBc", "AbC" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, _useSingleParamConstructor, _useSingleMessage, null);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(cfgStrings[0].Equals(cfgStrings[1]), Is.False);
        }

        [Test]
        public void Use_Set_Strategy_For_Comparison()
        {
            string[] rawValues = new[] { "a", "b", "c", "A", "B", "C" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, _useSingleParamConstructor, _useSingleMessage, StringComparison.InvariantCulture);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(actual, Is.EqualTo("aAbBcC"));
        }

        [Test]
        public void Use_Set_Strategy_For_Equality()
        {
            string[] rawValues = new[] { "aBc", "AbC" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, _useSingleParamConstructor, _useSingleMessage, StringComparison.OrdinalIgnoreCase);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(cfgStrings[0].Equals(cfgStrings[1]), Is.True);
        }

        [Test]
        public void After_Built_Throws_Exception()
        {
            ConfigurableString.Builder builder = Create(_useSingleParamConstructor, _useSingleMessage);

            _ = builder.Build("Some dummy, but valid, input.");

            Assert.That(() => builder.WithComparisonStrategy(StringComparison.OrdinalIgnoreCase),
                Throws.InstanceOf<InvalidOperationException>().With.Message.EqualTo(BuildMessage.AlreadyBuiltErrorMessage));
        }

        private static ConfigurableString[] FromRaw(in string[] rawValues, in bool useSingleParamConstructor, in bool useSingleMessage, in StringComparison? strategy)
        {
            List<ConfigurableString> result = new List<ConfigurableString>();
            foreach (string rawValue in rawValues)
            {
                ConfigurableString.Builder builder = Create(useSingleParamConstructor, useSingleMessage);

                if (strategy.HasValue)
                {
                    builder.WithComparisonStrategy(strategy.Value);
                }

                result.Add(builder.Build(rawValue));
            }

            return result.ToArray();
        }
    }
}