using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using GridPatternLibrary.Helpers.Abstract;
using GridPatternLibrary.Helpers.Concrete;
using GridPatternLibrary.Support;
using System.Linq;
using GridPatternLibrary.Xml;

namespace GridPatternLibrary
{
    public static class Connector
    {
        private static readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

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

        [DllExport]
        public static unsafe int GetData(string filePath, int* legs, MqlStr* actions, MqlStr* errors)
        {
            var dispatchedPettern = GetData(filePath);
            if (!dispatchedPettern.Success)
            {
                errors[0].SetString(dispatchedPettern.Error);
                return 0;
            }
            for (var i = 0; i < dispatchedPettern.Legs.Count; i++)
                for (var j = 0; j < dispatchedPettern.Legs[i].Count; j++)
                {
                    var index = UnsafeCodeHelper.Index(i, j, 16, 4);
                    legs[index] = dispatchedPettern.Legs[i][j];
                }
            for (var i = 0; i < dispatchedPettern.Actions.Count; i++)
                for (var j = 0; j < dispatchedPettern.Actions[i].Count; j++)
                {
                    var index = UnsafeCodeHelper.Index(i, j, 16, 5);
                    actions[index].SetString(dispatchedPettern.Actions[i][j]);
                }
            return 1;
        }

        public static DispatchedPattern GetData(string filePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"experts\files", filePath);
            string pattern;
            try
            {
                pattern = FileHelper.ReadFile(fullPath);
            }
            catch (FileNotFoundException)
            {
                return new DispatchedPattern(false, "Pattern file not found");
            }
            var parsedPattern = PatternParser.Parse(pattern);
            
            var validationFunctions = new List<Func<List<List<string>>, List<string>>>
                {
                   PatternValidator.IsSizeValid,
                   PatternValidator.IsPositionsValid,
                   PatternValidator.IsSyntaxValid,
                   PatternValidator.IsTypesValid,
                   PatternValidator.IsActionDuplicateValid
                };

            var error = string.Empty;

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

        [DllExport]
        public static unsafe int ParseActions(string actionsStr, MqlStr* actions, int* magics)
        {
            var tradeActions = ActionParser.Parse(actionsStr);
            for (var i = 0; i < tradeActions.Count; i++)
            {
                actions[i].SetString(tradeActions[i].Action);
                magics[i] = tradeActions[i].Magic;
            }
            return tradeActions.Count;
        }

        [DllExport]
        public static string GetPattern(int pattern, int sameLegCount)
        {
            var patterns = string.Empty;
            for (var i = pattern; i < pattern + sameLegCount; i++)
                patterns += PatternTransformer.IntToChar(i);

            return patterns;
        }

        [DllExport]
        public static int IsWatchedPattern(int pattern, string watchedPatterns)
        {
            foreach (var watchedPattern in watchedPatterns)
            {
                try
                {
                    if (pattern == PatternTransformer.CharToInt(watchedPattern.ToString(CultureInfo.InvariantCulture)))
                        return 1;
                }
                catch (ArgumentException)
                {
                }                
            }
            return 0;
        }

        [DllExport]
        public static unsafe void SaveSession(string appName, string recordId, MqlStr* keys, MqlStr* values, int count)
        {
            var appDataFileName = Functions.GetAppDataFileName(appName);
            var attributes = new Dictionary<string, string>();
            for (var index = 0; index < count; ++index)
                attributes.Add(keys[index].ToString(), values[index].ToString());
            recordId = recordId.Replace(" ", "");
            locker.EnterWriteLock();
            if (!File.Exists(appDataFileName))
                ToXml.CreateXmlDefaulFile(appDataFileName);
            XmlRecordAppender.AppendRecord(appDataFileName, recordId, attributes);
            locker.ExitWriteLock();
        }

        [DllExport]
        public static unsafe int LoadSession(string appName, string recordId, MqlStr* keys, MqlStr* values, int dataArraySize)
        {
            var appDataFileName = Functions.GetAppDataFileName(appName);
            locker.EnterReadLock();
            if (!File.Exists(appDataFileName))
            {
                locker.ExitReadLock();
                return 0;
            }
            recordId = recordId.Replace(" ", "");
            var dictionary = XmlRecordReader.ReadRecord(appDataFileName, recordId);
            for (var index = 0; index < dictionary.Count; ++index)
            {
                if (index >= dataArraySize)
                    break;
                keys[index].SetString(dictionary.Keys.ElementAt(index));
                values[index].SetString(dictionary.Values.ElementAt(index));
            }
            locker.ExitReadLock();
            return dictionary.Count;
        }

        [DllExport]
        public static void AppendToLog(string appName, int id, string info)
        {
            Log4Net.GetLogger(appName, id).Info(info);
        }

        [DllExport]
        public static void CloseLog(int id)
        {
            Log4Net.ShoutdownLogger(id);
        }
    }
}