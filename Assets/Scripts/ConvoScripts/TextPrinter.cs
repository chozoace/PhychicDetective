using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{
    Text _UIText;

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
        //loop through all text boxes
        Invoke("IncrementDisplayText", _typeSpeed);
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
            if (ConversationController.Instance().CurrentConvo._convoOutputList.Count > ConversationController.Instance().CurrentConvoIndex
                && ConversationController.Instance().CurrentConvo._convoOutputList[ConversationController.Instance().CurrentConvoIndex]._choiceOutputList.Count > 0)
            {
                //Have cursor with choices appear

                //change control to notebook
                GameState._conversationState.NotebookControl = true;
                GameState._pauseState.Enter();
            }
            else if(ConversationController.Instance().CurrentConvo._convoOutputList.Count > ConversationController.Instance().CurrentConvoIndex
                && ConversationController.Instance().CurrentConvo._convoOutputList[ConversationController.Instance().CurrentConvoIndex]._clueID != -1)
            {
                PlayerControllerScript.Instance().CollectInteractable(ConversationController.Instance().CurrentConvo._convoOutputList[ConversationController.Instance().CurrentConvoIndex]._clueID);
            }
        }
    }

    public void UpdateTextPrinter()
    {

    }
}
