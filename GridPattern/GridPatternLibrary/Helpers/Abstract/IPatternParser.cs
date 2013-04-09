using System.Collections.Generic;

namespace GridPatternLibrary.Helpers.Abstract
{
    public interface IPatternParser
    {
        List<List<string>> Parse(string data);

        string RemoveSpaces(string data);
        List<string> GetRows(string data);
        List<string> RemoveTitleRow(List<string> rows);
        List<List<string>> GetColumns(List<string> rows);
        List<List<string>> RemoveTitleColumn(List<List<string>> columns);
    }
}