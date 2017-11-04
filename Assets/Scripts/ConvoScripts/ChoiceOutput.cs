using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Choice")]
[System.Serializable]
public class ChoiceOutput
{
    [XmlAttribute("text")]
    public string _text;

    [XmlAttribute("type")]
    public string _type;

    [XmlAttribute("nextConvo")]
    public string _nextConvo;
}
