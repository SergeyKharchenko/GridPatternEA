using System.Collections.Generic;
using System.IO;
using System.Xml;
using GridPatternLibrary.Support;

namespace GridPatternLibrary.Xml
{
    public static class ToXml
    {
        public const string XmlForDefaultFile = "<products />";

        public static void CreateXmlDefaulFile(string fullFileName)
        {
            File.WriteAllText(fullFileName, XmlForDefaultFile);
        }

        public static XmlDocument CreateXmlDocument(string saveFileName)
        {
            var xmlDocument = new XmlDocument();
            try
            {
                var xml = File.ReadAllText(saveFileName);
                xmlDocument.LoadXml(xml);
            }
            catch (FileNotFoundException)
            {
            }
            return xmlDocument;
        }

        public static string LoadRecord(string appName, string recordId, string recordName)
        {
            var appDataFileName = Functions.GetAppDataFileName(appName);
            recordId = recordId.Replace(" ", "");
            var mutex = MutexExtension.GetMutex();
            mutex.Set(10000);

            if (File.Exists(appDataFileName))
            {
                var dictionary = XmlRecordReader.ReadRecord(appDataFileName, recordId);
                if (dictionary.Count > 0)
                {
                    if (dictionary.ContainsKey(recordName))
                    {
                        mutex.Release();
                        return dictionary[recordName];
                    }
                }
            }
            mutex.Release();
            return string.Empty;
        }

        public static void SaveRecord(string appName, string recordId, string recordName, string recordValue)
        {
            var appDataFileName = Functions.GetAppDataFileName(appName);
            recordId = recordId.Replace(" ", "");
            var attributes = new Dictionary<string, string> { { recordName, recordValue } };
            var mutex = MutexExtension.GetMutex();
            mutex.Set(10000);
            if (!File.Exists(appDataFileName))
                ToXml.CreateXmlDefaulFile(appDataFileName);
            XmlRecordAppender.AppendRecord(appDataFileName, recordId, attributes);
            mutex.Release();
        }
    }
}
