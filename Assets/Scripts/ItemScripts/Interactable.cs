using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Interactable : MonoBehaviour
{
    //Kinds of interactables: Items, Npc's, Actionables(doors), information
    //NPC
    protected bool _canInteract;
    protected bool _saveable = true;
    protected bool _exists = true;
    public bool InteractableExists { get { return _exists; } }
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
    public Dictionary<string, Conversation> _conversationDictionary = new Dictionary<string, Conversation>();

    void Start ()
    {
        if (_conversationDictionary.Count == 0)
        {
            _convoContainer = ConversationContainer.Load(_xml);

            foreach (Conversation convo in _convoContainer.conversationList)
            {
                _conversationDictionary.Add(convo._name, convo);
            }

            if (_conversationDictionary.ContainsKey("Default"))
                _currentConvo = _conversationDictionary["Default"];
            if (_conversationDictionary.ContainsKey(_currentConvo._nextConvo))
                _nextConvo = _conversationDictionary[_currentConvo._nextConvo];
        }
    }
    
    public virtual void onInteract()
    {
        if(_canInteract)
        {
            Debug.Log("Interacting with " + _itemName);
            
            GameObject.FindGameObjectWithTag("ConversationController").GetComponent<ConversationController>().SetConversationInfo(this);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._conversationState);     
        }
    }

    public virtual Conversation UpdateConvoInfo()
    {
        bool convoAssigned = false;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        foreach (KeyValuePair<string, Conversation> convo in _conversationDictionary)
        {
            //Debug.Log(convo.Value._checkForId);
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

    public Conversation AssignNewConvo(string newConvo)
    {
        Debug.Log(newConvo);
        _currentConvo = _conversationDictionary[newConvo];
        _nextConvo = _conversationDictionary[_currentConvo._nextConvo];
        _currentConvo._timesRead++;

        return _currentConvo;
    }

    public void DestroyInteractable()
    {
        if (PlayerControllerScript.Instance().CollidingInteractable == this)
        {
            PlayerControllerScript.Instance().CollidingInteractable = null;
            _canInteract = false;
        }
        _exists = false;
        gameObject.SetActive(false);
    }

    public void SaveData()
    {
        if (_saveable)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            if (File.Exists(Application.persistentDataPath + "/" + _itemName + ".dat"))
                file = File.Open(Application.persistentDataPath + "/" + _itemName + ".dat", FileMode.Open);
            else
                file = File.Open(Application.persistentDataPath + "/" + _itemName + ".dat", FileMode.Create);

            InteractableSerialize iser = new InteractableSerialize();
            iser._conversationDictionary = _conversationDictionary;
            iser._currentConvo = _currentConvo;
            iser._nextConvo = _nextConvo;
            iser._exists = _exists;

            bf.Serialize(file, iser);
            file.Close();
        }
    }

    public void LoadData()
    {
        Debug.Log(Application.persistentDataPath + "/" + _itemName + ".dat");
        if (File.Exists(Application.persistentDataPath + "/" + _itemName + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + _itemName + ".dat", FileMode.Open);
            InteractableSerialize iser = (InteractableSerialize)bf.Deserialize(file);

            _conversationDictionary = iser._conversationDictionary;
            _currentConvo = iser._currentConvo;
            _nextConvo = iser._nextConvo;
            _exists = iser._exists;
            gameObject.SetActive(_exists);
            
            file.Close();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<PlayerControllerScript>().CollidingInteractable == null)
        {
            other.GetComponent<PlayerControllerScript>().CollidingInteractable = this;
            _canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<PlayerControllerScript>().CollidingInteractable == this)
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

[System.Serializable]
public class InteractableSerialize
{
    public Dictionary<string, Conversation> _conversationDictionary = new Dictionary<string, Conversation>();
    public Conversation _currentConvo;
    public Conversation _nextConvo;
    public bool _exists;
    public int _xPos;
    public int _yPos;

    public InteractableSerialize()
    {

    }
}