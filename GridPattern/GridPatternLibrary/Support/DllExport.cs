using System;

namespace GridPatternLibrary.Support
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DllExport : Attribute
    {
    }
}