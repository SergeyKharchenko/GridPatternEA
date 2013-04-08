using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GridPatternLibrary.Xml
{
    public static class XmlRecordReader
    {
        public static Dictionary<string, string> ReadRecord(string loadFileName, string recordId)
        {
            var dictionary = new Dictionary<string, string>();
            try
            {
                using (
                    var reader = XmlReader.Create(new StringReader(File.ReadAllText(loadFileName))))
                    dictionary = GetAllAttributes(reader, recordId);
            }
            catch (XmlException)
            {
            }
            return dictionary;
        }

        private static Dictionary<string, string> GetAllAttributes(XmlReader reader, string recordId)
        {
            var dictionary = new Dictionary<string, string>();
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.Name != recordId) 
                    continue;
                while (reader.MoveToNextAttribute())
                {
                    var name = reader.Name;
                    var str = reader.Value;
                    dictionary.Add(name, str);
                }
                break;
            }
            return dictionary;
        }
    }
}