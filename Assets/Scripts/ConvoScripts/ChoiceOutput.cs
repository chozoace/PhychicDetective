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

    [XmlAttribute("nextConvo")]
    public string _nextConvo;
}
