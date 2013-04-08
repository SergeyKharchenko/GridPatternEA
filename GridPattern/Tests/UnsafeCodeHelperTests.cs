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
                new object[] {0, 0, 0},
                new object[] {1, 0, 4},
                new object[] {2, 1, 9},
                new object[] {14, 0, 56},
                new object[] {15, 3, 63}
            };

        private static readonly object[] indexesWithException =
            {
                new object[] {-1, 0},
                new object[] {16, 0},
                new object[] {15, 4}
            };

        [Test, TestCaseSource("indexes")]
        public void IndexTest(int x, int y, int result)
        {
            var index = UnsafeCodeHelper.Index(x, y);

            Assert.That(index, Is.EqualTo(result));
        }

        [Test, TestCaseSource("indexesWithException"), ExpectedException(typeof(ArgumentException))]
        public void IndexWithExceptionTest(int x, int y)
        {
            UnsafeCodeHelper.Index(x, y);
        }
    }
}