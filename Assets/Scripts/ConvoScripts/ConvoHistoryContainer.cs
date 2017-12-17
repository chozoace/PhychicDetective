using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("ConvoHistoryContainer")]
public class ConvoHistoryContainer
{
    [XmlArray("HistoryList")]
    [XmlArrayItem("Record")]
    public List<Record> _historyRecordList = new List<Record>();

    public static ConvoHistoryContainer Load(string xmlPath)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(xmlPath);
        XmlSerializer serializer = new XmlSerializer(typeof(ConvoHistoryContainer));
        StringReader reader = new StringReader(xDoc.OuterXml);
        ConvoHistoryContainer historyContainer = serializer.Deserialize(reader) as ConvoHistoryContainer;

        reader.Close();

        return historyContainer;
    }
}