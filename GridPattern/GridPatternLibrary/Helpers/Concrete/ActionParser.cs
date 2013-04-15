using System.Collections.Generic;
using System.Text.RegularExpressions;
using GridPatternLibrary.Helpers.Abstract;
using System.Linq;

namespace GridPatternLibrary.Helpers.Concrete
{
    public static class ActionParser
    {
        public static List<TradeAction> Parse(string actions)
        {
            const string pattern = @"(?<action>(?!;)\D+)(?<magic>\d+)";
            var regex = new Regex(pattern);
            var tradeActions = from match in regex.Matches(actions).Cast<Match>()
                              select new TradeAction(match.Groups["action"].Value,
                                                     int.Parse(match.Groups["magic"].Value));
            return tradeActions.ToList();
        }
    }
}