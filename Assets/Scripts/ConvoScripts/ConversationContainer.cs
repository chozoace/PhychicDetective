using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("ConversationCollection")]
public class ConversationContainer
{
    [XmlArray("Conversations")]
    [XmlArrayItem("Conversation")]
    public List<Conversation> conversationList = new List<Conversation>();

    public static ConversationContainer Load(TextAsset xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ConversationContainer));
        StringReader reader = new StringReader(xml.text);
        ConversationContainer conversationContainer = serializer.Deserialize(reader) as ConversationContainer;

        reader.Close();

        return conversationContainer;
    }
}
