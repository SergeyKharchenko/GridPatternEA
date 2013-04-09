using System.Collections.Generic;

namespace GridPatternLibrary.Helpers.Abstract
{
    public interface IPatternValidator
    {
        List<string> IsSizeValid(List<List<string>> data);
    }
}