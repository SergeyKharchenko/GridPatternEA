using System;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class UnsafeCodeHelperTests
    {
        private static readonly object[] indexes =
            {
                new object[] {0, 0, 16, 4, 0},
                new object[] {1, 0, 16, 4, 4},

                new object[] {2, 1, 16, 5, 11},
                new object[] {14, 0, 16, 5, 70},
                new object[] {15, 3, 16, 5, 78}
            };

        private static readonly object[] indexesWithException =
            {
                new object[] {-1, 0, 16, 4},
                new object[] {16, 0, 16, 4},
                new object[] {15, 5, 16, 5}
            };

        [Test, TestCaseSource("indexes")]
        public void IndexTest(int x, int y, int firstDimenshionSize, int secondDimensionSize, int result)
        {
            var index = UnsafeCodeHelper.Index(x, y, firstDimenshionSize, secondDimensionSize);

            Assert.That(index, Is.EqualTo(result));
        }

        [Test, TestCaseSource("indexesWithException"), ExpectedException(typeof(ArgumentException))]
        public void IndexWithExceptionTest(int x, int y, int firstDimenshionSize, int secondDimensionSize)
        {
            UnsafeCodeHelper.Index(x, y, firstDimenshionSize, secondDimensionSize);
        }
    }
}