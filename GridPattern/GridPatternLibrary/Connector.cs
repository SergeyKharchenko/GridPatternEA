using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GridPatternLibrary.Helpers.Abstract;
using GridPatternLibrary.Helpers.Concrete;
using GridPatternLibrary.Support;
using System.Linq;

namespace GridPatternLibrary
{
    public static class Connector
    {
        public static IFileHelper FileHelper { get; set; }
        public static IPatternParser PatternParser { get; set; }
        public static IPatternNormalizer PatternNormalizer { get; set; }
        public static IPatternValidator PatternValidator { get; set; }
        public static IPatternDispatcher PatternDispatcher { get; set; }

        static Connector()
        {
            FileHelper = new FileHelper();
            PatternParser = new PatternParser();
            PatternNormalizer = new PatternNormalizer();
            PatternValidator = new PatternValidator();
            PatternDispatcher = new PatternDispatcher();
        }

        [DllExport]
        public static void DllSleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        } 

        //[DllExport]
        //public static unsafe bool GetData(string filePath, int* legs, MqlStr* actions, MqlStr* errors)
        public static DispatchedPattern GetData(string filePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"experts\libraries", filePath);
            var pattern = FileHelper.ReadFile(fullPath);
            var parsedPattern = PatternParser.Parse(pattern);

            var validationFunctions = new List<Func<List<List<string>>, List<string>>>
                {
                    PatternValidator.IsSizeValid,
                    PatternValidator.IsPositionsValid,
                    PatternValidator.IsSyntaxValid,
                    PatternValidator.IsTypesValid,
                    PatternValidator.IsActionDuplicateValid
                };

            string error = string.Empty;
            if (!CheckOnValid(parsedPattern, validationFunctions, ref error))
                return new DispatchedPattern(false, error);

            var normalizedPattern = PatternNormalizer.TransferDownPattern(parsedPattern);

            validationFunctions.Clear();
            validationFunctions.Add(PatternValidator.IsCloseActionPositionValid);

            if (!CheckOnValid(normalizedPattern, validationFunctions, ref error))
                return new DispatchedPattern(false, error);

            return PatternDispatcher.Dispatch(normalizedPattern);
        }

        private static bool CheckOnValid(List<List<string>> pattern,
                                         IEnumerable<Func<List<List<string>>, List<string>>> validationFunctions,
                                         ref string error)
        {
            foreach (var err in validationFunctions.Select(validationFunction => validationFunction(pattern))
                                                   .Where(err => err.Count > 0))
            {
                error = err.First();
                return false;
            }
            return true;
        }
    }
}