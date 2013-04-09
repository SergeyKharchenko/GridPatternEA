using System;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PatternTransformerTests
    {
        private static readonly object[] intToCharData =
            {
                new object[] {1, "A"},
                new object[] {2, "B"},
                new object[] {13, "M"},
                new object[] {16, "P"},
            };

        [Test, TestCaseSource("intToCharData")]
        public void IntToCharTest(int index, string pattern)
        {
            var result = PatternTransformer.IntToChar(index);

            Assert.That(result, Is.EqualTo(pattern));
        }

        private static readonly object[] intToCharDataWithException =
            {
                new object[] {0, ""},
                new object[] {-1, ""},
                new object[] {17, ""}
            };

        [Test, TestCaseSource("intToCharDataWithException"), ExpectedException(typeof(ArgumentException))]
        public void IntToCharWithExceptionTest(int index, string pattern)
        {
            var result = PatternTransformer.IntToChar(index);

            Assert.That(result, Is.EqualTo(pattern));
        }

        private static readonly object[] charToIntData =
            {
                new object[] {"A", 1},
                new object[] {"B", 2},
                new object[] {"M", 13},
                new object[] {"P", 16},
            };

        [Test, TestCaseSource("charToIntData")]
        public void CharToIntTest(string pattern, int index)
        {
            var result = PatternTransformer.CharToInt(pattern);

            Assert.That(result, Is.EqualTo(index));
        }

        private static readonly object[] charToIntDataWithException =
            {
                new object[] {"a", 1},
                new object[] {"Q", 2},
                new object[] {"Z", 13}
            };

        [Test, TestCaseSource("charToIntDataWithException"), ExpectedException(typeof(ArgumentException))]
        public void CharToIntWithExceptionTest(string pattern, int index)
        {
            var result = PatternTransformer.CharToInt(pattern);

            Assert.That(result, Is.EqualTo(index));
        }
    }
}