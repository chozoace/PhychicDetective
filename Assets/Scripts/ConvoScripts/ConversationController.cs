using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

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
    static ConversationController _instance;

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

    public void CheckConversationForChange()
    {
        
    }

    public void EndConversation()
    {
        //fade everything away
        _conversationBackground.SetActive(false);
    }

    void Awake ()
    {
        _textPrinter = GetComponent<TextPrinter>();
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    void LoadConvoBlurb()
    {
        //possibly load choice blurb?
        if (_currentConvoIndex < _currentConvo._convoOutputList.Count)
        {
            //set speaking char sprite to convo info
            if(_currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite != null)
            {
                Debug.Log(_currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite);
                _speakingCharSprite.GetComponent<PortraitScript>().ActivatePortrait("Sprites/" + _currentConvo._convoOutputList[_currentConvoIndex]._speakerSprite);
            }
            else
            {
                _speakingCharSprite.GetComponent<PortraitScript>().DisablePortrait();
            }

            string textToPrint = _currentConvo._convoOutputList[_currentConvoIndex]._speaker + ": " + _currentConvo._convoOutputList[_currentConvoIndex]._speech;
            //string textChoice1 = _currentConvo._convoOutputList[_currentConvoIndex]._speaker + ": " + _currentConvo._convoOutputList[_currentConvoIndex]._choiceOutputList[0]._text;
            //string textChoice2 = _currentConvo._convoOutputList[_currentConvoIndex]._speaker + ": " + _currentConvo._convoOutputList[_currentConvoIndex]._choiceOutputList[1]._text;
            _textPrinter.TextToType = textToPrint;
            //_textPrinter._textChoice1 = textChoice1;
            //_textPrinter._textChoice2 = textChoice2;
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
            Destroy(_conversationInfo.gameObject);
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
            Debug.Log("Correct");
            //get  
            _currentConvo = _conversationInfo.AssignNewConvo(_currentConvo._name + "ItemCorrect");
            _currentConvoIndex = 0;
            LoadConvoBlurb();
        }
        else
        {
            Debug.Log("wrong");
            _currentConvo = _conversationInfo.AssignNewConvo(_currentConvo._name + "ItemWrong");
            _currentConvoIndex = 0;
            LoadConvoBlurb();
        }
    }

    public void UpdateConversation()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //go to next convo blurb
            if (_textPrinter.NumberOfLettersToShow < _textPrinter.TextToType.Length - 1)
                _textPrinter.NumberOfLettersToShow = _textPrinter.TextToType.Length - 1;
            else
                LoadConvoBlurb();
        }
        _textPrinter.UpdateTextPrinter();
    }
}