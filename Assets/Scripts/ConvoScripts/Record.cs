using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("Record")]
[System.Serializable]
public class Record
{
    [XmlAttribute("speaker")]
    public string _speaker;
    [XmlAttribute("speech")]
    public string _speech;
}

