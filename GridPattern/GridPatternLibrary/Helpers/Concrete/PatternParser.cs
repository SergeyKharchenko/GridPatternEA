using System;
using System.Collections.Generic;
using GridPatternLibrary.Helpers.Abstract;
using System.Linq;

namespace GridPatternLibrary.Helpers.Concrete
{
    public class PatternParser : IPatternParser
    {
        public List<List<string>> Parse(string data)
        {
            data = RemoveSpaces(data);
            var rows = GetRows(data);
            rows = RemoveTitleRow(rows);
            var columns = GetColumns(rows);
            columns = RemoveTitleColumn(columns);
            return columns;
        }

        public string RemoveSpaces(string data)
        {
            return data.Replace(" ", "");
        }

        public List<string> GetRows(string data)
        {
            return data.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public List<string> RemoveTitleRow(List<string> rows)
        {
            rows.RemoveAt(0);
            return rows;
        }

        public List<List<string>> GetColumns(List<string> rows)
        {
            return rows.Select(row => row.Split(new[] {","}, StringSplitOptions.None).ToList()).ToList();
        }

        public List<List<string>> RemoveTitleColumn(List<List<string>> columns)
        {
            foreach (var row in columns.Where(row => row.Count > 0))
                row.RemoveAt(0);
            return columns;
        }
    }
}