using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{
    Text _UIText;
    GameObject _ChoiceUI;
    GameObject _cursor;
    Text _ChoiceHeader;
    Text _Choice1;
    Text _Choice2;
    Text _Choice3;
    ConversationController _convoController;

    string _textToType;
    public string TextToType { set { _textToType = value; } get { return _textToType; } }
    [SerializeField] float _typeSpeed = 1f;
    private float _textPercentage = 0;
    public float TextPercentage { get { return _textPercentage; } }
    int _numberOfLettersToShow = 0;
    int _numberOfLettersToShowChoices = 0;
    public int NumberOfLettersToShowChoices { set { _numberOfLettersToShowChoices = value; } get { return _numberOfLettersToShowChoices; } }
    public int NumberOfLettersToShow { set { _numberOfLettersToShow = value; } get { return _numberOfLettersToShow; } }
    [SerializeField] GameObject _conversationBackground;

    int _selectedChoiceEntry = 1;
    int _currentChoiceLength = 0;
    float _cursorStartPosition;

    List<Text> _textUIs = new List<Text>();

    void Start()
    {
        _UIText = _conversationBackground.transform.Find("Text").gameObject.GetComponent<Text>();
        _ChoiceUI = _conversationBackground.transform.Find("ChoiceUI").gameObject;
        _ChoiceHeader = _ChoiceUI.transform.Find("ChoiceHeader").gameObject.GetComponent<Text>();
        _textUIs.Add(_ChoiceUI.transform.Find("ChoiceHeader").gameObject.GetComponent<Text>());
        _textUIs.Add(_ChoiceUI.transform.Find("Choice1").gameObject.GetComponent<Text>());
        _textUIs.Add(_ChoiceUI.transform.Find("Choice2").gameObject.GetComponent<Text>());
        _textUIs.Add(_ChoiceUI.transform.Find("Choice3").gameObject.GetComponent<Text>());

        _cursor = _ChoiceUI.transform.Find("Cursor").gameObject;
        //_cursorStartPosition = _cursor.gameObject.transform.position.y;
    }

    public void StartTyper(string typerFunction = "IncrementDisplayText")
    {
        Invoke(typerFunction, _typeSpeed);
        if (_convoController == null)
            _convoController = ConversationController.Instance();
    }

    public bool cursorActive()
    {
        return _cursor.activeInHierarchy;
    }

    public void ClearTyper()
    {
        _textPercentage = 0;
        _numberOfLettersToShow = 0;
        _numberOfLettersToShowChoices = 0;
        _UIText.text = "";
        _ChoiceHeader.text = "";
        foreach (Text text in _textUIs)
            text.text = "";
        CancelInvoke();
    }

    public void increaseChoiceSelection()
    {
        if(_selectedChoiceEntry < _currentChoiceLength)
        {
            _selectedChoiceEntry++;
            Vector2 cursorPosition = _cursor.transform.position;
            cursorPosition.y -= .24f;
            _cursor.transform.position = cursorPosition;
        }
    }

    void resetChoiceSelection()
    {
        while(_selectedChoiceEntry > 1)
        {
            decreaseChoiceSelection();
        }
    }

    public void decreaseChoiceSelection()
    {
        if (_selectedChoiceEntry > 1)
        {
            _selectedChoiceEntry--;
            Vector2 cursorPosition = _cursor.transform.position;
            cursorPosition.y += .24f;
            _cursor.transform.position = cursorPosition;
        }
    }

    public string selectChoiceSelection()
    {
        _convoController.SetChoicesAvailable = false;
        _cursor.SetActive(false);
        ChoiceOutput choiceObj = _convoController.CurrentConvo._convoOutputList[_convoController.CurrentConvoIndex]._choiceOutputList[_selectedChoiceEntry];
        return choiceObj._nextConvo;
    }

    IEnumerator IncrementChoiceText(string textToType, int choiceIndex)
    {
        Text textTargetObj = _textUIs[choiceIndex];
        while (_numberOfLettersToShow < textToType.Length)
        {
            _numberOfLettersToShow++;
            textTargetObj.text = textToType.Substring(0, _numberOfLettersToShow);
            yield return new WaitForSeconds(_typeSpeed);
        }
        DisplayChoicesText(++choiceIndex);
    }

    void DisplayChoicesText(int choiceIndex)
    {
        ConvoOutput currentConvoOutput = _convoController.CurrentConvo._convoOutputList[_convoController.CurrentConvoIndex];
        _currentChoiceLength = currentConvoOutput._choiceOutputList.Count - 1;
        if (choiceIndex < currentConvoOutput._choiceOutputList.Count)
        {
            _numberOfLettersToShow = 0;
            ChoiceOutput choiceObj = currentConvoOutput._choiceOutputList[choiceIndex];
            string textToType = (choiceIndex == 0) ? (currentConvoOutput._speaker + ": " + choiceObj._text) : choiceObj._text;
            _textToType = textToType;
            this.StartCoroutine(IncrementChoiceText(textToType, choiceIndex));
        }
        else
        {
            _cursor.SetActive(true);
            resetChoiceSelection();
        }
    }

    void IncrementDisplayText()
    {
        _ChoiceUI.SetActive(false);
        Conversation currentConvo = _convoController.CurrentConvo;
        if (_numberOfLettersToShow < _textToType.Length)
        {
            if (currentConvo._convoOutputList.Count > _convoController.CurrentConvoIndex
                && currentConvo._convoOutputList[_convoController.CurrentConvoIndex]._choiceOutputList.Count > 0)
            {
                _ChoiceUI.SetActive(true);
                _convoController.SetChoicesAvailable = true;
                resetChoiceSelection();
                DisplayChoicesText(0);
            }
            else
            {
                _numberOfLettersToShow++;
                _UIText.text = _textToType.Substring(0, _numberOfLettersToShow);
                Invoke("IncrementDisplayText", _typeSpeed);
            }
        }
        else
        {
            //to collect clues
            if(currentConvo._convoOutputList.Count > _convoController.CurrentConvoIndex
                && currentConvo._convoOutputList[_convoController.CurrentConvoIndex]._clueID != -1)
            {
                PlayerControllerScript.Instance().CollectInteractable(currentConvo._convoOutputList[_convoController.CurrentConvoIndex]._clueID);
            }
        }
    }
}
