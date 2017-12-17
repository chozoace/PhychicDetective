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
    GameObject _itemPickupUIPrefab;
    bool _interactableMenu = false;

    string _historyDocPath = "Assets/Resources/convoHistory.xml";
    XmlDocument _xDoc = new XmlDocument();
    XmlNode _historyListNode;
    static ConversationController _instance;
    static GameController _gameController;
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
        _itemPickupUIPrefab = (GameObject)Resources.Load("CollectionsUI");
        XmlNode root;
        //if file doesn't exist, create it
        if (File.Exists(_historyDocPath))
        {
            _xDoc.Load(_historyDocPath);
            _xDoc.DocumentElement.RemoveAll();
            root = _xDoc.DocumentElement;
        }
        else
        {
            Debug.Log("not exists");
            _xDoc = new XmlDocument();
            root = _xDoc.CreateElement("ConvoHistoryContainer");
            _xDoc.AppendChild(root);
        }
        _historyListNode = _xDoc.CreateElement("HistoryList");
        root.AppendChild(_historyListNode);
        _xDoc.Save(_historyDocPath);

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
            ConvoOutput currentConvoOutput = _currentConvo._convoOutputList[_currentConvoIndex];
            //set speaking char sprite to convo info
            if (_currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite != null)
            {
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
                _xDoc.Save(_historyDocPath);
                
                //include special animation changes in skip typer?
                _textPrinter.TextToType = textToPrint;
                _textPrinter.ClearTyper();
                _textPrinter.StartTyper();
            }
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
            case "CollectableItem":
                GameObject uiInstance = Instantiate(_itemPickupUIPrefab);
                //pass it the item to display in the UI
                uiInstance.transform.SetParent(PlayerControllerScript.Instance().gameObject.transform.parent.FindChild("Canvas"), false);
                uiInstance.GetComponent<CollectionUIScript>().setItem(_conversationInfo.GetItemID);
                Animation animUIIntance = uiInstance.GetComponent<Animation>();
                animUIIntance["CollectionsUIAnim"].wrapMode = WrapMode.Once;
                animUIIntance.Play("CollectionsUIAnim");

                _playerGameObject.GetComponent<PlayerControllerScript>().CollectInteractable(_conversationInfo.GetItemID);
                _conversationInfo.DestroyInteractable();
                _textPrinter.TextToType = "";
                _textPrinter.ClearTyper();
                _currentConvoIndex = 0;
                _interactableMenu = true;
                _conversationBackground.SetActive(false);
                break;
            case "NonCollectableItem":
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
        else if (_interactableMenu)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Destroy(PlayerControllerScript.Instance().gameObject.transform.parent.FindChild("Canvas").FindChild("CollectionsUI(Clone)").gameObject);
                _gameController.ChangeGameState(GameState._overworldState);
                _interactableMenu = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (_textPrinter.NumberOfLettersToShow < _textPrinter.TextToType.Length - 1)
                    _textPrinter.NumberOfLettersToShow = _textPrinter.TextToType.Length - 1;
                else
                {
                    _currentConvoIndex++;
                    LoadConvoBlurb();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._historyPauseState);
            }
        }
    }
}