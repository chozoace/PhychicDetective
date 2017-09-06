using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class ConversationController : MonoBehaviour
{
    public Interactable _conversationInfo;
    GameObject _textBox;
    ConversationContainer _convoContainer;
    Conversation _currentConvo;
    public Conversation CurrentConvo { get { return _currentConvo; } }
    TextPrinter _textPrinter;
    int _currentConvoIndex = 0;
    public int CurrentConvoIndex { get { return _currentConvoIndex; } }
    int _currentConvoSpeechIndex = 0;
    public string _currentConvoName;
    string _postConvoAction;
    [SerializeField] GameObject _playerGameObject;
    [SerializeField] GameObject _conversationBackground;
    [SerializeField] GameObject _speakingCharSprite;
    XmlDocument _xDoc = new XmlDocument();
    XmlNode _historyListNode;
    static ConversationController _instance;

    string _convoHistoryString = "Assets/Resources/convoHistory.txt";

    public static ConversationController Instance()
    {
        if (_instance != null)
            return _instance;
        else
            throw new System.ArgumentException("Convo Controller instance is null");
    }

    public void SetConversationInfo(Interactable conversationInfo)
    {
        _conversationInfo = conversationInfo;
        //extract info
    }

    public void StartConversation()
    {
        //set up text box and actors
        _conversationBackground.SetActive(true);
        _currentConvo = _conversationInfo.UpdateConvoInfo();

        Debug.Log("Current Convo: " + _currentConvo._name);

        _postConvoAction = _currentConvo._postConvoAction;
        LoadConvoBlurb();
    }

    public void EndConversation()
    {
        //fade everything away
        _conversationBackground.SetActive(false);
    }
    
    void OnDestroy()
    {
        _xDoc.Save("Assets/Resources/convoHistory.xml");
    }

    void Awake ()
    {
        _textPrinter = GetComponent<TextPrinter>();
        //Clear history
        _xDoc.Load("Assets/Resources/convoHistory.xml");
        _xDoc.DocumentElement.RemoveAll();
        XmlNode root = _xDoc.DocumentElement;
        _historyListNode = _xDoc.CreateElement("HistoryList");
        root.AppendChild(_historyListNode);
        
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    void LoadConvoBlurb()
    {
        PortraitScript speakingPortrait = _speakingCharSprite.GetComponent<PortraitScript>();
        //possibly load choice blurb?
        if (_currentConvoIndex < _currentConvo._convoOutputList.Count)
        {
            ConvoOutput currentConvoOutput = _currentConvo._convoOutputList[_currentConvoIndex];
            //set speaking char sprite to convo info
            if (_currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite != null)
            {
                Debug.Log(_currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite);
                speakingPortrait.ActivatePortrait("Sprites/" + currentConvoOutput._speakerSprite, currentConvoOutput._speaker, currentConvoOutput._emotion);
            }
            else
            {
                speakingPortrait.DisablePortrait();
            }

            string textToPrint = currentConvoOutput._speaker + ": " + currentConvoOutput._speech; 
            //save to history

            
            _textPrinter.TextToType = textToPrint;
            _textPrinter.ClearTyper();
            _textPrinter.StartTyper();
            _currentConvoIndex++;
        }
        else
        {
            CheckPostConvoInfo();
            _textPrinter._textToType = "";
            _textPrinter.ClearTyper();
            _currentConvoIndex = 0;
            _currentConvoSpeechIndex = 0;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._overworldState);
        }

    }
    
    void CheckPostConvoInfo()
    {
        if (_postConvoAction == "Item")
        {
            _playerGameObject.GetComponent<PlayerControllerScript>().CollectInteractable(_conversationInfo._itemId);
            _conversationInfo.DestroyInteractable();
        }
        if(_postConvoAction == "Profile")
        {
            _playerGameObject.GetComponent<PlayerControllerScript>().CollectInteractable(_conversationInfo._itemId);
        }
    }

    public void PresentEvidence(int itemId)
    {
        if(int.Parse(_currentConvo._convoOutputList[_currentConvoIndex]._needEvidence) == itemId)
        { 
            _currentConvo = _conversationInfo.AssignNewConvo(_currentConvo._name + "ItemCorrect");
            _currentConvoIndex = 0;
            LoadConvoBlurb();
        }
        else
        {
            _currentConvo = _conversationInfo.AssignNewConvo(_currentConvo._name + "ItemWrong");
            _currentConvoIndex = 0;
            LoadConvoBlurb();
        }
    }

    public void UpdateConversation()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (_textPrinter.NumberOfLettersToShow < _textPrinter.TextToType.Length - 1)
                _textPrinter.NumberOfLettersToShow = _textPrinter.TextToType.Length - 1;
            else
                LoadConvoBlurb();
        }
        _textPrinter.UpdateTextPrinter();
    }
}