using NUnit.Framework;
using Translator.Library;

namespace Translator.Tests
{
    public class NumericalTranslatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(0, "Zero")]
        [TestCase(0.0, "Zero")]
        [TestCase(-0, "Zero")]
        [TestCase(-0.0, "Zero")]
        public void ParseNumberWithZero(double input, string expected_output)
        {
            var translator = new NumericalTranslator(input);
            var output = translator.GetOutput();
            Assert.AreEqual(expected_output, output);
        }

        [Test]
        [TestCase(1000005, "One Million Five")]
        [TestCase(1000205, "One Million Two Hundred Five")]
        [TestCase(6002205, "Six Million Two Thousand Two Hundred Five")]
        [TestCase(6022205, "Six Million Twenty-Two Thousand Two Hundred Five")]
        [TestCase(1000000, "One Million")]
        [TestCase(1000000000, "One Billion")]
        [TestCase((double)10000000000, "Ten Billion")]
        public void ParseNumberWholeNumber(double input, string expected_output)
        {
            var translator = new NumericalTranslator(input);
            var output = translator.GetOutput();
            Assert.AreEqual(expected_output, output);
        }

        [Test]
        [TestCase(-0.1, "Negative One Tenth")]
        [TestCase(0.0, "Zero")]
        [TestCase(0.1, "One Tenth")]
        [TestCase(0.00001, "One Hundred-Thousandth")]
        public void ParseNumberDecimalOnly(double input, string expected_output)
        {
            var translator = new NumericalTranslator(input);
            var output = translator.GetOutput();
            Assert.AreEqual(expected_output, output);
        }

        [Test]
        [TestCase(1001.01, "One Thousand One and One Hundredth")]
        [TestCase(5021000.022, "Five Million Twenty-One Thousand and Twenty-Two Thousandths")]
        [TestCase(-70000.0, "Negative Seventy Thousand")]
        [TestCase(-70000.0005, "Negative Seventy Thousand and Five Ten-Thousandths")]
        public void ParseNumberWithWholeAndDecimal(double input, string expected_output)
        {
            var translator = new NumericalTranslator(input);
            var output = translator.GetOutput();
            Assert.AreEqual(expected_output, output);
        }
    }
}