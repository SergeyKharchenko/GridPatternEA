using System;

namespace GridPatternLibrary.Helpers.Concrete
{
    public static class UnsafeCodeHelper
    {
        public static int Index(int x, int y, int firstDimenshionSize, int secondDimensionSize)
        {
            var index = x * secondDimensionSize + y;
            if (index > (firstDimenshionSize * secondDimensionSize - 1) || index < 0)
                throw new ArgumentException("Index is invalid");
            return index;
        }
    }
}