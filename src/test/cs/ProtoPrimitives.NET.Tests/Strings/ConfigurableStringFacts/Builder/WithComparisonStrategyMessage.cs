using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProtoPrimitives.NET.Strings;
using static ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder.ConstructorMessage;

namespace ProtoPrimitives.NET.Tests.Strings.ConfigurableStringFacts.Builder
{
    [TestFixture]
    internal sealed class WithComparisonStrategyMessage
    {
        private static readonly IEnumerable<StringComparison> AllStrategies = Enum.GetValues(typeof(StringComparison)).Cast<StringComparison>().ToList();

        [Test]
        public void With_Valid_Values_Throws_Nothing(
            [ValueSource(nameof(AllStrategies))] StringComparison strategy,
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = Create(useSingleParamConstructor, ArgNullErrorMessage, useSingleMessage);

            Assert.That(() => builder.WithComparisonStrategy(strategy), Throws.Nothing);
        }

        [Test]
        public void With_Invalid_Values_Throws_ArgumentOutOfRangeException(
            [Values(-1, 6)] StringComparison strategy,
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = Create(useSingleParamConstructor, ArgNullErrorMessage, useSingleMessage);

            Assert.That(() => builder.WithComparisonStrategy(strategy),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName)).EqualTo("comparisonStrategy")
                    .And.Property(nameof(ArgumentOutOfRangeException.ActualValue)).EqualTo(strategy));
        }

        [Test]
        public void Can_Be_Called_Twice(
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            var builder = Create(useSingleParamConstructor, ArgNullErrorMessage, useSingleMessage);

            builder.WithComparisonStrategy(StringComparison.InvariantCulture);

            Assert.That(() => builder.WithComparisonStrategy(StringComparison.Ordinal), Throws.Nothing);
        }

        [Test]
        public void Use_Default_Strategy_For_Comparison(
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            string[] rawValues = new[] { "a", "b", "c", "A", "B", "C" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, useSingleParamConstructor, useSingleMessage, null);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(actual, Is.EqualTo("ABCabc"));
        }

        [Test]
        public void Use_Default_Strategy_For_Equality(
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            string[] rawValues = new[] { "aBc", "AbC" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, useSingleParamConstructor, useSingleMessage, null);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(cfgStrings[0].Equals(cfgStrings[1]), Is.False);
        }

        [Test]
        public void Use_Set_Strategy_For_Comparison(
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            string[] rawValues = new[] { "a", "b", "c", "A", "B", "C" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, useSingleParamConstructor, useSingleMessage, StringComparison.InvariantCulture);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(actual, Is.EqualTo("aAbBcC"));
        }

        [Test]
        public void Use_Set_Strategy_For_Equality(
            [Values(false, true)] in bool useSingleParamConstructor,
            [Values(false, true)] in bool useSingleMessage)
        {
            Assume.That(useSingleParamConstructor && useSingleMessage, Is.Not.True);

            string[] rawValues = new[] { "aBc", "AbC" };
            ConfigurableString[] cfgStrings = FromRaw(rawValues, useSingleParamConstructor, useSingleMessage, StringComparison.OrdinalIgnoreCase);

            string actual = string.Join("", cfgStrings.OrderBy(cfgStr => cfgStr).Select(cfgStr => cfgStr.Value));

            Assert.That(cfgStrings[0].Equals(cfgStrings[1]), Is.True);
        }

        private ConfigurableString[] FromRaw(in string[] rawValues, in bool useSingleParamConstructor, in bool useSingleMessage, in StringComparison? strategy)
        {
            List<ConfigurableString> result = new List<ConfigurableString>();
            foreach (string rawValue in rawValues)
            {
                var builder = Create(useSingleParamConstructor, ArgNullErrorMessage, useSingleMessage);

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