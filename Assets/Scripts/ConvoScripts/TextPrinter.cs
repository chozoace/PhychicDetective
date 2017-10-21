using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{
    Text _UIText;
    ConversationController _convoController;

    public string _textToType;
    public string _textChoice1;
    public string _textChoice2;
    public string TextToType { set { _textToType = value; } get { return _textToType; } }
    public float _typeSpeed = 1f;
    private float _textPercentage = 0;
    public float TextPercentage { get { return _textPercentage; } }
    int _numberOfLettersToShow = 0;
    public int NumberOfLettersToShow { set { _numberOfLettersToShow = value; } get { return _numberOfLettersToShow; } }
    [SerializeField] GameObject _conversationBackground;

    void Start()
    {
        _UIText = _conversationBackground.transform.Find("Text").gameObject.GetComponent<Text>();
    }

    public void StartTyper()
    {
        Invoke("IncrementDisplayText", _typeSpeed);
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

    void IncrementDisplayText()
    {
        if (_numberOfLettersToShow < _textToType.Length)
        {
            _numberOfLettersToShow++;
            _UIText.text = _textToType.Substring(0, _numberOfLettersToShow);
            Invoke("IncrementDisplayText", _typeSpeed);
        }
        else
        {
            if (_convoController.CurrentConvo._convoOutputList.Count > _convoController.CurrentConvoIndex
                && _convoController.CurrentConvo._convoOutputList[_convoController.CurrentConvoIndex]._choiceOutputList.Count > 0)
            {
                //Have cursor with choices appear
                //call controller function to change to control options, the controller will call typer again to type out options then be given control

                //change control to notebook
                GameState._conversationState.NotebookControl = true;
                GameState._pauseState.Enter();
            }
            //to collect clues
            else if(_convoController.CurrentConvo._convoOutputList.Count > _convoController.CurrentConvoIndex
                && _convoController.CurrentConvo._convoOutputList[_convoController.CurrentConvoIndex]._clueID != -1)
            {
                PlayerControllerScript.Instance().CollectInteractable(_convoController.CurrentConvo._convoOutputList[_convoController.CurrentConvoIndex]._clueID);
            }
        }
    }

    public void UpdateTextPrinter()
    {

    }
}
