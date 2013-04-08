using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GridPattern.library.Support
{
    public struct MqlStr
    {
        public int MqlLength;
        public IntPtr MqlString;

        public new string ToString()
        {
            return Marshal.PtrToStringAnsi(MqlString);
        }

        public void SetString(string value)
        {
            var bytes = Encoding.Default.GetBytes((value + char.MinValue).ToCharArray());
            Marshal.Copy(bytes, 0, MqlString, bytes.Length);
        } 
    }
}