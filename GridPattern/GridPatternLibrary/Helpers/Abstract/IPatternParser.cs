namespace GridPatternLibrary.Helpers.Abstract
{
    public interface IPatternParser
    {
        string RemoveSpaces(string data);
        string RemoveFirstRow(string data);
        string[] GetRows(string data);
        string[] RemoveFirstColumn(string[] data);
    }
}