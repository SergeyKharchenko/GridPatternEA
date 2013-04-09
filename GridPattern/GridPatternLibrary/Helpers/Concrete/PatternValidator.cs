using System.Collections.Generic;
using GridPatternLibrary.Helpers.Abstract;

namespace GridPatternLibrary.Helpers.Concrete
{
    public class PatternValidator : IPatternValidator
    {
        private const int PatternCount = 16;

        public List<string> IsSizeValid(List<List<string>> data)
        {
            var errors = new List<string>();

            if (data.Count != 16)
                errors.Add("Patter count is invalid");

            for (var i = 0; i < data.Count; i++)
            {
                //if ()
            }
            return errors;
        }
    }
}