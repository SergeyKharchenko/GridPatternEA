using System.Collections.Generic;
using GridPatternLibrary.Helpers.Abstract;
using System.Linq;

namespace GridPatternLibrary.Helpers.Concrete
{
    public class PatternValidator : IPatternValidator
    {
        private const int PatternCount = 16;
        private const int PatternRowLength = 9;

        public List<string> IsSizeValid(List<List<string>> pattern)
        {
            var errors = new List<string>();

            if (pattern.Count != PatternCount)
                errors.Add("Pattern row count is invalid");


            for (var i = 0; i < pattern.Count; i++)
            {
                var row = pattern[i];
                if (row.Count != PatternRowLength)
                {
                    errors.Add(string.Format("Pattern row: {0} is invalid", PatternTransformer.IntToChar(i)));
                }
            }
            return errors;
        }
    }
}