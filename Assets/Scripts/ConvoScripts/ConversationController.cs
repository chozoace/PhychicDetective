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
    TextPrinter _textPrinter;
    int _currentConvoIndex = 0;
    int _currentConvoSpeechIndex = 0;
    public string _currentConvoName;
    string _postConvoAction;
    [SerializeField] GameObject _playerGameObject;
    [SerializeField] GameObject _conversationBackground;

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
	}

    void LoadConvoBlurb()
    {
        if (_currentConvoIndex < _currentConvo._convoOutputList.Count)
        {
            string textToPrint = _currentConvo._convoOutputList[_currentConvoIndex]._speaker + ": " + _currentConvo._convoOutputList[_currentConvoIndex]._speech;
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
            Destroy(_conversationInfo.gameObject);
        }
        if(_postConvoAction == "Profile")
        {
            _playerGameObject.GetComponent<PlayerControllerScript>().CollectInteractable(_conversationInfo._itemId);
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