using System.Collections.Generic;

namespace GridPatternLibrary.Helpers.Abstract
{
    public interface IPatternDispatcher
    {
        DispatchedPattern Dispatch(List<List<string>> pattern);
    }
}