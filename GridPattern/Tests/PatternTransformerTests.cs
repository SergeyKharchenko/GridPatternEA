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
                new object[] {0, "A"},
                new object[] {1, "B"},
                new object[] {12, "M"},
                new object[] {15, "P"},
            };

        [Test, TestCaseSource("intToCharData")]
        public void IntToCharTest(int index, string pattern)
        {
            var result = PatternTransformer.IntToChar(index);

            Assert.That(result, Is.EqualTo(pattern));
        }

        private static readonly object[] intToCharDataWithException =
            {
                new object[] {-1, ""},
                new object[] {16, ""},
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
                new object[] {"A", 0},
                new object[] {"B", 1},
                new object[] {"M", 12},
                new object[] {"P", 15},
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

        private static readonly object[] getTransferDownLastRowIndexData =
            {
                new object[] {0, 0, 15},
                new object[] {0, 2, 7},
                new object[] {0, 4, 3},
                new object[] {0, 6, 1},
                new object[] {8, 2, 15},
                new object[] {8, 4, 11},
                new object[] {12, 6, 13},
                new object[] {15, 8, 15},
            };

        [Test, TestCaseSource("getTransferDownLastRowIndexData")]
        public void GetTransferDownLastRowIndexTest(int rowIndex, int columnIndex, int lastRowIndex)
        {
            var result = PatternTransformer.GetTransferDownLastRowIndex(rowIndex, columnIndex);

            Assert.That(result, Is.EqualTo(lastRowIndex));
        }
    }
}