using System;

namespace GridPattern.library.Support
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DllExport : Attribute
    {
    }
}