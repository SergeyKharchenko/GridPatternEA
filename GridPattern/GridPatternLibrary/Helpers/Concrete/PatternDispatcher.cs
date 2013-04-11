using System;
using System.Collections.Generic;
using GridPatternLibrary.Helpers.Abstract;

namespace GridPatternLibrary.Helpers.Concrete
{
    public class PatternDispatcher : IPatternDispatcher
    {
        public DispatchedPattern Dispatch(List<List<string>> pattern)
        {
            var legs = new List<List<int>>();
            var actions = new List<List<string>>();

            foreach (var row in pattern)
            {
                var rowLegs = new List<int>();
                var rowActions = new List<string>();
                
                var isLeg = true;
                foreach (var column in row)
                {
                    isLeg = !isLeg;
                    if (isLeg)
                    {
                        var leg = int.Parse(column);
                        rowLegs.Add(leg);
                    }
                    else
                    {
                        var currentActions = column.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        rowActions.AddRange(currentActions);
                    }
                }
                legs.Add(rowLegs);
                actions.Add(rowActions);
            }

            return new DispatchedPattern(true, string.Empty, legs, actions);
        }
    }
}