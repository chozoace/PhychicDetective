using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


public class Conversation
{
    [XmlAttribute("name")]
    public string _name;

    [XmlAttribute("postConvoAction")]
    public string _postConvoAction;

    [XmlAttribute("nextConvo")]
    public string _nextConvo;

    [XmlAttribute("checkForType")]
    public string _checkForType;

    [XmlAttribute("checkForId")]
    public string _checkForId;

    public int _timesRead = 0;

    [XmlArray("Script")]
    [XmlArrayItem("ConvoOutput")]
    public List<ConvoOutput> _convoOutputList = new List<ConvoOutput>();
}
