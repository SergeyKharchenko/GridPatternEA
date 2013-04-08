using System.Collections.Generic;
using System.Xml;

namespace GridPattern.library.Xml
{
    public static class XmlRecordCleaner
    {
        private const int RecordCountLimit = 100;

        public static void CleanFile(string fullFileName)
        {
            var xmlDocument = ToXml.CreateXmlDocument(fullFileName);
            foreach (var oldChild in GetUnlimitedNodes(xmlDocument))
                xmlDocument.FirstChild.RemoveChild(oldChild);
            xmlDocument.Save(fullFileName);
        }

        private static IEnumerable<XmlNode> GetUnlimitedNodes(XmlNode xmlDocument)
        {
            var list = new List<XmlNode>();
            var count = xmlDocument.FirstChild.ChildNodes.Count;
            for (var index = 0; index < count - RecordCountLimit; ++index)
                list.Add(xmlDocument.FirstChild.ChildNodes[index]);
            return list;
        }
    }
}
