using System.Threading;
using GridPattern.library.Support;

namespace GridPattern.library
{
    public class Connector
    {
        [DllExport]
        public static void DllSleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        } 
    }
}