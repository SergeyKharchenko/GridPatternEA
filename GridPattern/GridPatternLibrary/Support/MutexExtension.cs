using System;
using System.Threading;

namespace GridPatternLibrary.Support
{
    public static class MutexExtension
    {
        private const string MutexName = "GridEA_Mutex";

        public static void Set(this WaitHandle mutex, int millisecond)
        {
            try
            {
                mutex.WaitOne(millisecond);
            }
            catch (AbandonedMutexException)
            {
            }
        }

        public static void Release(this Mutex mutex)
        {
            try
            {
                mutex.ReleaseMutex();
            }
            catch (ApplicationException)
            {
            }
        }

        public static Mutex GetMutex()
        {
            Mutex mutex;
            try
            {
                mutex = Mutex.OpenExisting(MutexName);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                mutex = new Mutex(false, MutexName);
            }
            return mutex;
        }
    }
}