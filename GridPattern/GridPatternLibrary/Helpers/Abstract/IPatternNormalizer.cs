using System.Collections.Generic;

namespace GridPatternLibrary.Helpers.Abstract
{
    public interface IPatternNormalizer
    {
        List<List<string>> TransferDownPattern(List<List<string>> pattern);
    }
}