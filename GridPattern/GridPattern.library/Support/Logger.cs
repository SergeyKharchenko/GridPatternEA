using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GridPattern.library.Support
{
    public static class Logger
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole(); 

        public enum SaveMode
        {
            None   = -1,
            Master = 0,
            Client = 1,
        }

        static Logger()
        {
            CreateDebug(true);
        }

        private static void CreateDebug(bool withConsole, string traceFileName = "", bool withTime = true)
        {          
            console = withConsole;
            time = withTime;
            if (console)
            {
                AllocConsole();
                Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            }
            
            if (traceFileName != "")
            {
                var listener = new TextWriterTraceListener(traceFileName, traceFileName);
                Debug.Listeners.Add(listener);
            }
        }

        public static void AddTraceFile(string traceFileName)
        {
            var listener = new TextWriterTraceListener(traceFileName, traceFileName);
            Debug.Listeners.Add(listener);
        }

        public static void RemoveTraceFile(string traceFileName)
        {
            Debug.Listeners.Remove(traceFileName);
        }

        public static void RemoveAllTraceFiles()
        {
            for (int i = Debug.Listeners.Count - 1; i >= 0 ; i--)
            {
                if (console && i == 0)
                {
                    break;
                }
                Debug.Listeners.RemoveAt(i);
            }
        }

        public static void WriteLine(object message)
        {
            Debug.WriteLine(time
                                ? string.Format("{0}:{1}:{2} - {3}", DateTime.UtcNow.ToUniversalTime(),
                                                DateTime.UtcNow.Second, DateTime.UtcNow.Millisecond, message)
                                : string.Format("{0}", message));


            Debug.Flush();
        }


        public static void DeleteAllTraceFiles()
        {
            for (var i = Debug.Listeners.Count - 1; i >= 0; i--)
            {
                if (console && i == 0)
                {
                    break;
                }
                Debug.Listeners[i].Dispose();
                File.Delete(Debug.Listeners[i].Name); 
            }
            RemoveAllTraceFiles();
        }
        
        public static void DeleteTraceFile(string traceFileName)
        {
            foreach (var textWriterTraceListener in Debug.Listeners.Cast<object>().OfType<TextWriterTraceListener>().
                Where(textWriterTraceListener => textWriterTraceListener.Name == traceFileName))
            {
                textWriterTraceListener.Dispose();
            }

            Debug.Listeners.Remove(traceFileName);
            File.Delete(traceFileName);          
        }


        private static bool console;
        private static bool time;
    }
}
