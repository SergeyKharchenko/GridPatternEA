using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GridPatternLibrary.Xml
{
    public static class XmlRecordAppender
    {
        public static void AppendRecord(string saveFileName, string recordId, Dictionary<string, string> attributes)
        {
            var xmlDocument = ToXml.CreateXmlDocument(saveFileName);
            var elementsByTagName = xmlDocument.GetElementsByTagName(recordId);
            if (elementsByTagName.Count > 0)
                ChangeRecord(xmlDocument, elementsByTagName[0], attributes);
            else
                CreateRecord(xmlDocument, recordId, attributes);
            try
            {
                XmlRecordCleaner.CleanFile(saveFileName);
                xmlDocument.Save(saveFileName);
            }
            catch (XmlException)
            {
            }
        }

        private static void ChangeRecord(XmlDocument xmlDocument, XmlNode xmlNode, IDictionary<string, string> attributes)
        {
            if (xmlNode.Attributes != null)
            {
                for (var i = attributes.Count - 1; i >= 0; --i)
                {
                    var xmlAttribute =
                        xmlNode.Attributes.Cast<XmlAttribute>()
                               .FirstOrDefault(
                                   currentAttribute =>
                                   currentAttribute.Name ==
                                   attributes.Keys.ElementAt(i));
                    if (xmlAttribute == null) continue;
                    xmlAttribute.Value = attributes.Values.ElementAt(i);
                    attributes.Remove(attributes.Keys.ElementAt(i));
                }
            }
            AppendAttributes(xmlDocument, xmlNode, attributes);
        }

        private static void AppendAttributes(XmlDocument xmlDocument, XmlNode xmlNode, IEnumerable<KeyValuePair<string, string>> attributes)
        {
            foreach (var keyValuePair in attributes)
            {
                try
                {
                    var attribute = xmlDocument.CreateAttribute(keyValuePair.Key);
                    attribute.Value = keyValuePair.Value;
                    if (xmlNode.Attributes != null)
                        xmlNode.Attributes.Append(attribute);
                }
                catch (XmlException)
                {
                }
            }
        }

        private static void CreateRecord(XmlDocument xmlDocument, string recordId, Dictionary<string, string> attributes)
        {
            try
            {
                var firstChild = xmlDocument.FirstChild;
                var node = xmlDocument.CreateNode(XmlNodeType.Element, recordId, string.Empty);
                AppendAttributes(xmlDocument, node, attributes);
                firstChild.AppendChild(node);
            }
            catch (XmlException)
            {
            }
        }
    }
}
