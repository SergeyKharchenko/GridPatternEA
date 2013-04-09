using System;
using System.Globalization;

namespace GridPatternLibrary.Helpers.Concrete
{
    public static class PatternTransformer
    {
        private const int StartCharPos = 64;

        public static string IntToChar(int index)
        {
            if (index < 1 || index > 16)
                throw new ArgumentException("Pattern index is invalid");
            index += StartCharPos;            
            return ((char) index).ToString(CultureInfo.InvariantCulture);
        }
        public static int CharToInt(string pattern)
        {
            if (pattern.Length != 1) 
                throw new ArgumentException("Pattern is invalid");
            var index = pattern[0] - StartCharPos;
            if (index < 1 || index > 16)
                throw new ArgumentException("Pattern index is invalid");
            return index;
        }
    }
}