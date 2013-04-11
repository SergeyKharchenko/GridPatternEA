using System.Collections.Generic;

namespace GridPatternLibrary
{
    public class DispatchedPattern
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }
        public List<List<int>> Legs { get; private set; }
        public List<List<string>> Actions { get; private set; }

        public DispatchedPattern(bool success, string error = "", List<List<int>> legs = null, List<List<string>> actions = null)
        {
            Success = success;
            Error = error;
            Legs = legs;
            Actions = actions;
        }
    }
}