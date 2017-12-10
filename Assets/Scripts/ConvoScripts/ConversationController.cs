using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class ConversationController : MonoBehaviour
{
    Interactable _conversationInfo;
    GameObject _textBox;
    ConversationContainer _convoContainer;
    Conversation _currentConvo;
    public Conversation CurrentConvo { get { return _currentConvo; } }
    TextPrinter _textPrinter;
    int _currentConvoIndex = 0;
    public int CurrentConvoIndex { get { return _currentConvoIndex; } }
    string _postConvoAction;
    [SerializeField] GameObject _playerGameObject;
    [SerializeField] GameObject _conversationBackground;
    [SerializeField] GameObject _speakingCharSprite;

    XmlDocument _xDoc = new XmlDocument();
    XmlNode _historyListNode;
    static ConversationController _instance;
    static GameController _gameController;
    string _convoHistoryString = "Assets/Resources/convoHistory.txt";
    bool _choicesAvailable = false;
    public bool SetChoicesAvailable { set { _choicesAvailable = value; } }

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
    }

    public void StartConversation()
    {
        _conversationBackground.SetActive(true);
        _currentConvo = _conversationInfo.UpdateConvoInfo();

        LoadConvoBlurb();
    }

    public void EndConversation()
    {
        _conversationBackground.SetActive(false);
    }
    
    void OnDestroy()
    {
        _xDoc.Save("Assets/Resources/convoHistory.xml");
    }

    void Awake ()
    {
        _textPrinter = GetComponent<TextPrinter>();
        _gameController = GameController.Instance();

        _xDoc.Load("Assets/Resources/convoHistory.xml");
        _xDoc.DocumentElement.RemoveAll();
        XmlNode root = _xDoc.DocumentElement;
        _historyListNode = _xDoc.CreateElement("HistoryList");
        root.AppendChild(_historyListNode);
        _xDoc.Save("Assets/Resources/convoHistory.xml");

        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    void LoadConvoBlurb(bool skipTyper = false)
    {
        _postConvoAction = _currentConvo._postConvoAction;
        PortraitScript speakingPortrait = _speakingCharSprite.GetComponent<PortraitScript>();
        if (_currentConvoIndex < _currentConvo._convoOutputList.Count)
        {
            Debug.Log("changing emotion");
            ConvoOutput currentConvoOutput = _currentConvo._convoOutputList[_currentConvoIndex];
            //set speaking char sprite to convo info
            if (_currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite != null)
            {
                Debug.Log("activating " + currentConvoOutput._emotion);
                speakingPortrait.ActivatePortrait("Sprites/" + currentConvoOutput._speakerSprite, currentConvoOutput._speaker, currentConvoOutput._emotion);
            }
            else
                speakingPortrait.DisablePortrait();

            if (!skipTyper)
            {
                string textToPrint = currentConvoOutput._speaker + ": " + currentConvoOutput._speech;
                //don't record history of items
                XmlElement el = _xDoc.CreateElement("Record");
                el.SetAttribute("speaker", currentConvoOutput._speaker);
                el.SetAttribute("speech",  currentConvoOutput._speech);
                _historyListNode.AppendChild(el);
                _xDoc.Save("Assets/Resources/convoHistory.xml");
                //include special animation changes in skip typer?
                _textPrinter.TextToType = textToPrint;
                _textPrinter.ClearTyper();
                _textPrinter.StartTyper();
            }
            _currentConvoIndex++;
        }
        else
        {
            CheckPostConvoInfo();
        }

    }
    
    void CheckPostConvoInfo()
    {
        switch(_postConvoAction)
        {
            case "Item":
                _playerGameObject.GetComponent<PlayerControllerScript>().CollectInteractable(_conversationInfo.GetItemID);
                _conversationInfo.DestroyInteractable();
                _textPrinter.TextToType = "";
                _textPrinter.ClearTyper();
                _currentConvoIndex = 0;
                _gameController.ChangeGameState(GameState._overworldState);
                break;
            case "Profile":
                _playerGameObject.GetComponent<PlayerControllerScript>().CollectInteractable(_conversationInfo.GetItemID);
                _textPrinter.TextToType = "";
                _textPrinter.ClearTyper();
                _currentConvoIndex = 0;
                _gameController.ChangeGameState(GameState._overworldState);
                break;
            case "Notebook":
                GameState._conversationState.NotebookControl = true;
                GameState._pauseState.Enter();
                break;
            default:
                _textPrinter.TextToType = "";
                _textPrinter.ClearTyper();
                _currentConvoIndex = 0;
                _gameController.ChangeGameState(GameState._overworldState);
                break;
        }
    }

    public void PresentEvidence(int itemId)
    {
        if(int.Parse(_currentConvo._convoOutputList[_currentConvoIndex - 1]._needEvidence) == itemId)
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

    public void saveConversation()
    {
        _conversationBackground.SetActive(false);
    }
    
    public void loadConversation()
    {
        _conversationBackground.SetActive(true);
        _currentConvoIndex -= 1;
        LoadConvoBlurb(true);
    }

    public void UpdateConversation()
    {
        if (_choicesAvailable)
        {
            if (Input.GetKeyDown(KeyCode.S) && _textPrinter.cursorActive())
            {
                //down
                _textPrinter.increaseChoiceSelection();
            }
            if (Input.GetKeyDown(KeyCode.W) && _textPrinter.cursorActive())
            {
                //up
                _textPrinter.decreaseChoiceSelection();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (_textPrinter.NumberOfLettersToShow < _textPrinter.TextToType.Length - 1)
                {
                    _textPrinter.NumberOfLettersToShow = _textPrinter.TextToType.Length - 1;
                }
                else
                {
                    _currentConvo = _conversationInfo.AssignNewConvo(_textPrinter.selectChoiceSelection());
                    _currentConvoIndex = 0;
                    LoadConvoBlurb();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (_textPrinter.NumberOfLettersToShow < _textPrinter.TextToType.Length - 1)
                    _textPrinter.NumberOfLettersToShow = _textPrinter.TextToType.Length - 1;
                else
                    LoadConvoBlurb();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._historyPauseState);
                //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        _textPrinter.UpdateTextPrinter();
    }
}