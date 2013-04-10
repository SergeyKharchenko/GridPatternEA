using System.Collections.Generic;

namespace GridPatternLibrary.Helpers.Abstract
{
    public interface IPatternValidator
    {
        List<string> IsSizeValid(List<List<string>> pattern);
        List<string> IsPositionsValid(List<List<string>> pattern);
        List<string> IsSyntaxValid(List<List<string>> pattern);
        List<string> IsCloseActionPositionValid(List<List<string>> pattern);
        List<string> IsActionDuplicateValid(List<List<string>> pattern);
    }
}