using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("ConvoHistoryContainer")]
public class ConvoHistoryContainer
{
    [XmlArray("HistoryList")]
    [XmlArrayItem("Record")]
    public List<Record> historyRecordList = new List<Record>();

    public static ConvoHistoryContainer Load(TextAsset xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ConvoHistoryContainer));
        StringReader reader = new StringReader(xml.text);
        ConvoHistoryContainer historyContainer = serializer.Deserialize(reader) as ConvoHistoryContainer;

        reader.Close();

        return historyContainer;
    }
}