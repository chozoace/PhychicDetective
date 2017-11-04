using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{
    Text _UIText;
    GameObject _ChoiceUI;
    Text _ChoiceHeader;
    Text _Choice1;
    Text _Choice2;
    Text _Choice3;
    ConversationController _convoController;

    public string _textToType;
    public string _textChoice1;
    public string _textChoice2;
    public string TextToType { set { _textToType = value; } get { return _textToType; } }
    public float _typeSpeed = 1f;
    private float _textPercentage = 0;
    public float TextPercentage { get { return _textPercentage; } }
    int _numberOfLettersToShow = 0;
    int _numberOfLettersToShowChoices = 0;
    public int NumberOfLettersToShow { set { _numberOfLettersToShow = value; } get { return _numberOfLettersToShow; } }
    [SerializeField] GameObject _conversationBackground;

    int _selectedChoiceEntry = 1;
    int _currentChoiceLength = 0;

    void Start()
    {
        _UIText = _conversationBackground.transform.Find("Text").gameObject.GetComponent<Text>();
        _ChoiceUI = _conversationBackground.transform.Find("ChoiceUI").gameObject;
        _ChoiceHeader = _ChoiceUI.transform.Find("ChoiceHeader").gameObject.GetComponent<Text>();
        _Choice1 = _ChoiceUI.transform.Find("Choice1").gameObject.GetComponent<Text>();
        _Choice2 = _ChoiceUI.transform.Find("Choice2").gameObject.GetComponent<Text>();
        _Choice3 = _ChoiceUI.transform.Find("Choice3").gameObject.GetComponent<Text>();
    }

    public void StartTyper(string typerFunction = "IncrementDisplayText")
    {
        Invoke(typerFunction, _typeSpeed);
        if (_convoController == null)
            _convoController = ConversationController.Instance();
    }

    public void ClearTyper()
    {
        _textPercentage = 0;
        _numberOfLettersToShow = 0;
        _numberOfLettersToShowChoices = 0;
        _UIText.text = "";
        _ChoiceHeader.text = "";
        _Choice1.text = "";
        _Choice2.text = "";
        _Choice3.text = "";
        CancelInvoke();
    }

    public void increaseChoiceSelection()
    {
        if(_selectedChoiceEntry < _currentChoiceLength)
        {
            _selectedChoiceEntry++;
            //change cursor position
        }
    }

    public void decreaseChoiceSelection()
    {
        if (_selectedChoiceEntry > 1)
        {
            _selectedChoiceEntry--;
            //change cursor position
        }
    }

    public void selectChoiceSelection()
    {
        //return next convo string use it to load next convo via controller
    }

    IEnumerator IncrementChoiceText(string textTarget, string textToType, int choiceIndex)
    {
        Text textTargetObj = _ChoiceUI.transform.Find(textTarget).gameObject.GetComponent<Text>();
        while (_numberOfLettersToShowChoices < textToType.Length)
        {
            _numberOfLettersToShowChoices++;
            textTargetObj.text = textToType.Substring(0, _numberOfLettersToShowChoices);
            yield return new WaitForSeconds(_typeSpeed);
        }
        DisplayChoicesText(++choiceIndex);
    }

    void DisplayChoicesText(int choiceIndex)
    {
        ConvoOutput currentConvoOutput = _convoController.CurrentConvo._convoOutputList[_convoController.CurrentConvoIndex-1];
        _numberOfLettersToShowChoices = 0;
        _currentChoiceLength = currentConvoOutput._choiceOutputList.Count;
        if (choiceIndex < currentConvoOutput._choiceOutputList.Count)
        {
            ChoiceOutput choiceObj = currentConvoOutput._choiceOutputList[choiceIndex];
            string textToType = (choiceIndex == 0) ? (currentConvoOutput._speaker + ": " + choiceObj._text) : choiceObj._text;
            this.StartCoroutine(IncrementChoiceText(choiceObj._type, textToType, choiceIndex));
        }
        else
        {

        }
    }

    void IncrementDisplayText()
    {
        _ChoiceUI.SetActive(false);
        Conversation currentConvo = _convoController.CurrentConvo;
        if (_numberOfLettersToShow < _textToType.Length)
        {
            if (currentConvo._convoOutputList.Count > _convoController.CurrentConvoIndex
                && currentConvo._convoOutputList[_convoController.CurrentConvoIndex-1]._choiceOutputList.Count > 0)
            {
                //Have cursor with choices appear
                //call controller function to change to control options, the controller will call typer again to type out options then be given control
                _ChoiceUI.SetActive(true);
                _selectedChoiceEntry = 1;
                DisplayChoicesText(0);
                //change control to notebook
                //GameState._conversationState.NotebookControl = true;
                //GameState._pauseState.Enter();
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

    public void UpdateTextPrinter()
    {

    }
}
