using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public List<string> IsPositionsValid(List<List<string>> pattern)
        {
            var errors = new List<string>();

            var positions = new List<int> {9, 2, 4, 2, 6, 2, 4, 2, 8, 2, 4, 2, 6, 2, 4, 2};

            for (var i = 0; i < pattern.Count; i++)
            {
                var filledCount = positions[i];
                for (var j = PatternRowLength - 1; j >= 0; j--)
                {
                    if (string.IsNullOrEmpty(pattern[i][j]) && filledCount != 0)
                    {
                        errors.Add(string.Format("Invalid element position in pattern row {0}", PatternTransformer.IntToChar(i)));
                        break;
                    }
                    if (!string.IsNullOrEmpty(pattern[i][j]))
                        filledCount--;
                }
            }

            return errors;
        }

        public List<string> IsSyntaxValid(List<List<string>> pattern)
        {
            var errors = new List<string>();

            var validActionPatterns = new List<string>
                {
                    @"(;|^)BX(?!0)[0-9]{1,2}(;|$)",
                    @"(;|^)B(?!0)[0-9]{1,2}(;|$)",
                    @"(;|^)SX(?!0)[0-9]{1,2}(;|$)",
                    @"(;|^)S(?!0)[0-9]{1,2}(;|$)",
                    @"(;|^)N(;|$)",
                    @"(;|^)\-?\d+(;|$)",
                    @";",
                };

            var patternCopy = pattern.Select(row => new List<string>(row)).ToList();

            validActionPatterns.Aggregate(patternCopy, ClearValidAction);

            for (var i = 0; i < patternCopy.Count; i++)
            {
                errors.AddRange(from column in patternCopy[i]
                                where !string.IsNullOrEmpty(column)
                                select string.Format("Invalid action in pattern row {0}: {1}",
                                                     PatternTransformer.IntToChar(i), column));
            }

            return errors;
        }

        private static List<List<string>> ClearValidAction(List<List<string>> pattern, string validActionPattern)
        {
            var regex = new Regex(validActionPattern);
            foreach (var row in pattern)
                for (var j = 0; j < row.Count; j++)
                    row[j] = regex.Replace(row[j], "", 1);
            return pattern;
        }

        public List<string> IsTypesValid(List<List<string>> pattern)
        {
            var errors = new List<string>();

            for (var i = 0; i < pattern.Count; i++)
            {
                var isLeg = true;
                for (var j = 0; j < pattern[i].Count; j++)
                {
                    isLeg = !isLeg;
                    if (string.IsNullOrEmpty(pattern[i][j]))
                        continue;

                    
                    if (isLeg)
                    {
                        int parseResult;
                        if (!int.TryParse(pattern[i][j], out parseResult))
                            errors.Add(string.Format("Invalid element type in pattern row {0}: {1}",
                                                 PatternTransformer.IntToChar(i), pattern[i][j]));
                    }
                    else
                    {
                        var actions = pattern[i][j].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var action in actions)
                        {
                            int parseResult;
                            if (int.TryParse(action, out parseResult))
                                errors.Add(string.Format("Invalid element type in pattern row {0}: {1}",
                                                     PatternTransformer.IntToChar(i), action));
                        }
                    }
                }
            }

            return errors;
        }


        public List<string> IsActionDuplicateValid(List<List<string>> pattern)
        {
            var errors = new List<string>();

            var actionPatterns = new List<string>
                {
                    @"BX(?!0)[0-9]{1,2}",
                    @"B(?!0)[0-9]{1,2}",
                    @"SX(?!0)[0-9]{1,2}",
                    @"S(?!0)[0-9]{1,2}"
                };

            foreach (var regex in actionPatterns.Select(actionPattern => new Regex(actionPattern)))
            {
                for (var i = 0; i < pattern.Count; i++)
                {
                    for (var j = 0; j < pattern[i].Count; j++)
                    {
                        foreach (var currentAction in regex.Matches(pattern[i][j])
                                                           .Cast<Match>()
                                                           .Select(match => match.Groups[0].Value))
                        {
                            int duplicateRowIndex;
                            var found = FindDuplicate(pattern, i, j, currentAction, out duplicateRowIndex);
                            if (found)
                                errors.Add(string.Format("Duplicate action in pattern row {0}: {1}",
                                                         PatternTransformer.IntToChar(duplicateRowIndex), currentAction));
                        }
                    }
                }
            }

            return errors;
        }

        private static bool FindDuplicate(List<List<string>> pattern, int rowIndex, int columnIndex, string currentAction, out int duplicateRowIndex)
        {
            var found = false;
            duplicateRowIndex = 0;

            var lastRowIndex = PatternTransformer.GetTransferDownLastRowIndex(rowIndex, columnIndex);
            for (var i = rowIndex; i <= lastRowIndex; i++)
            {
                for (var j = columnIndex + 1; j < PatternRowLength; j++)
                {
                    var actions = pattern[i][j].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (actions.Any(action => action == currentAction))
                    {
                        duplicateRowIndex = i;
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        public List<string> IsCloseActionPositionValid(List<List<string>> pattern)
        {
            var errors = new List<string>();

            var closePatterns = new List<string>
                {
                    @"BX\d+",
                    @"SX\d+",
                };

            foreach (var regex in closePatterns.Select(closePattern => new Regex(closePattern)))
            {
                for (var i = 0; i < pattern.Count; i++)
                {
                    for (var j = 0; j < pattern[i].Count; j++)
                    {
                        var matchCollection = regex.Matches(pattern[i][j]);
                        errors.AddRange(from Match match in matchCollection
                                        let openAction = match.Groups[0].Value.Replace("X", "")
                                        let found = FindOpenAction(pattern, i, j, openAction)
                                        where !found
                                        select string.Format("Invalid close action position in pattern row {0}: {1}",
                                                             PatternTransformer.IntToChar(i), match.Groups[0].Value));
                    }
                }
            }

            return errors;
        }

        private static bool FindOpenAction(List<List<string>> pattern, int rowIndex, int columnIndex, string openAction)
        {
            var found = false;
            for (var k = columnIndex - 1; k >= 0; k--)
            {
                var actions = pattern[rowIndex][k].Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                if (actions.Any(action => action == openAction))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}