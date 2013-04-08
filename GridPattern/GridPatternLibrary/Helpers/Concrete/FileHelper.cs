using System.IO;
using GridPatternLibrary.Helpers.Abstract;

namespace GridPatternLibrary.Helpers.Concrete
{
    public class FileHelper : IFileHelper
    {
         public string ReadFile(string path)
         {
             return File.ReadAllText(path);
         }
    }
}