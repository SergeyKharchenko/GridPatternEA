using System.IO;
using System.Threading;
using GridPatternLibrary.Helpers.Abstract;
using GridPatternLibrary.Support;

namespace GridPatternLibrary
{
    public class Connector
    {
        public static IFileHelper FileHelper { get; set; }

        [DllExport]
        public static void DllSleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        } 

        [DllExport]
        //public static unsafe bool GetData(string filePath, int* legs, MqlStr* actions)
        public static bool GetData(string filePath, int[,] legs, string[,] actions)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"experts\libraries", filePath);
            var pattern = FileHelper.ReadFile(fullPath);
            return true;
        } 
    }
}