using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("ConvoOutput")]
[System.Serializable]
public class ConvoOutput
{
    [XmlAttribute("position")]
    public string _position;

    [XmlAttribute("needEvidence")]
    public string _needEvidence;

    [XmlElement("Speaker")]
    public string _speaker;

    [XmlElement("Speech")]
    public string _speech;

    [XmlArray("Choices")]
    [XmlArrayItem("Choice")]
    public List<ChoiceOutput> _choiceOutputList = new List<ChoiceOutput>();
}
