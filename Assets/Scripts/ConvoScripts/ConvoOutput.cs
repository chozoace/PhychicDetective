using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("ConvoOutput")]
[System.Serializable]
public class ConvoOutput
{
    [XmlAttribute("position")]
    public string _position;

    [XmlElement("Speaker")]
    public string _speaker;

    [XmlElement("Speech")]
    public string _speech;
}
