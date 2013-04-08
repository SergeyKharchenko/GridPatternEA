using System;

namespace GridPatternLibrary.Helpers.Concrete
{
    public static class UnsafeCodeHelper
    {
        public const int FirstDimensionSize = 16;
        public const int SecondDimensionSize = 4;

        public static int Index(int x, int y)
        {
            var index = x*SecondDimensionSize + y;
            if (index > 63 || index < 0) 
                throw new ArgumentException("Index is invalid");
            return index;
        }
    }
}