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

    [XmlAttribute("clueID")]
    public int _clueID = -1;

    [XmlAttribute("needEvidence")]
    public string _needEvidence;

    [XmlElement("Speaker")]
    public string _speaker;

    [XmlElement("SpeakerSprite")]
    public string _speakerSprite;

    [XmlElement("Speech")]
    public string _speech;

    [XmlArray("Choices")]
    [XmlArrayItem("Choice")]
    public List<ChoiceOutput> _choiceOutputList = new List<ChoiceOutput>();
}
