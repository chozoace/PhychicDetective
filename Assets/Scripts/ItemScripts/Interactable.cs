using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;


public class Interactable : MonoBehaviour
{
    //Kinds of interactables: Items, Npc's, Actionables(doors), information
    bool _canInteract;
    public string _itemName = "Default";
    public int _itemId;
    string _conversationOutput;
    //not public
    [SerializeField] TextAsset _xml;
    public TextAsset GetXML { get { return _xml; } }
    Conversation _currentConvo;
    public Conversation GetCurrentConvo { get { return _currentConvo; } }
    Conversation _nextConvo;
    public Conversation GetNextConvo { get { return _nextConvo; } }
    ConversationContainer _convoContainer;
    public ConversationContainer GetConvoContainer { get { return _convoContainer; } }
    Dictionary<string, Conversation> _conversationDictionary = new Dictionary<string, Conversation>();

    void Start ()
    {
        _convoContainer = ConversationContainer.Load(_xml);

        foreach (Conversation convo in _convoContainer.conversationList)
        {
            _conversationDictionary.Add(convo._name, convo);
        }

        _currentConvo = _conversationDictionary["Default"];
        if(_conversationDictionary.ContainsKey(_currentConvo._nextConvo))
            _nextConvo = _conversationDictionary[_currentConvo._nextConvo];
	}

    public virtual void onInteract()
    {
        if(_canInteract)
        {
            Debug.Log("Interacting with " + _itemName);
            
            ConversationController.Instance().SetConversationInfo(this);
            GameController.Instance().ChangeGameState(GameState._conversationState);     
        }
    }

    public virtual Conversation UpdateConvoInfo()
    {
        Debug.Log("checking update " + _itemName);
        bool convoAssigned = false;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        foreach (KeyValuePair<string, Conversation> convo in _conversationDictionary)
        {
            Debug.Log(convo.Value._checkForId);
            if (playerGameObject.GetComponent<PlayerControllerScript>().InventoryContains(convo.Value._checkForType, int.Parse(convo.Value._checkForId)) && convo.Value._timesRead == 0)
            {
                _currentConvo = convo.Value;
                _nextConvo = _conversationDictionary[convo.Value._nextConvo];
                _currentConvo._timesRead++;
                convoAssigned = true;
                break;
            }
        }
        if (!convoAssigned)
        {

            if (_currentConvo._timesRead > 0 && _conversationDictionary.ContainsKey(_currentConvo._nextConvo))
            {
                _currentConvo = _nextConvo;
                _nextConvo = _conversationDictionary[_currentConvo._nextConvo];
            }
            _currentConvo._timesRead++;
        }
        return _currentConvo;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerControllerScript>().CollidingInteractable = this;
            _canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerControllerScript>().CollidingInteractable = null;
            _canInteract = false;
        }
    }
	
	void Update ()
    {
	    //update only when this is the chosen interactable object
	}
}
