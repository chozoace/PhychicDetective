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
        _UIText.text = "";
        CancelInvoke();
    }

    void IncrementChoiceText(Text textTarget, string textToType)
    {
        if (_numberOfLettersToShowChoices < textToType.Length)
        {
            _numberOfLettersToShowChoices++;
            textTarget.text = textToType.Substring(0, _numberOfLettersToShow);
            Invoke("IncrementChoiceText", _typeSpeed);
        }
        else
        {
            //call display choices text
        }
    }

    void DisplayChoicesText()
    {
        Conversation currentConvo = _convoController.CurrentConvo;
        _ChoiceUI.SetActive(true);
        

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
                //Have cursor with choices appear
                //call controller function to change to control options, the controller will call typer again to type out options then be given control
                DisplayChoicesText();
                //change control to notebook
                GameState._conversationState.NotebookControl = true;
                GameState._pauseState.Enter();
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
