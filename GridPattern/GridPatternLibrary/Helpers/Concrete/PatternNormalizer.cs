using System.Collections.Generic;
using GridPatternLibrary.Helpers.Abstract;

namespace GridPatternLibrary.Helpers.Concrete
{
    public class PatternNormalizer : IPatternNormalizer
    {
        public List<List<string>> TransferDownPattern(List<List<string>> pattern)
        {
            for (var i = 0; i < pattern.Count; i++)
            {
                for (var j = 0; j < pattern[0].Count; j++)
                {
                    if (!string.IsNullOrEmpty(pattern[i][j]))
                        for (var k = i + 1; k < pattern.Count; k++)
                        {
                            if (!string.IsNullOrEmpty(pattern[k][j]))
                                break;
                            pattern[k][j] = pattern[i][j];
                        }
                }
            }
            return pattern;
        }
    }
}