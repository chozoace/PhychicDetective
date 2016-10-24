using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{
    public Text _UIText;

    public string _textToType;
    public string TextToType { set { _textToType = value; } get { return _textToType; } }
    public float _typeSpeed = 1f;
    private float _textPercentage = 0;
    public float TextPercentage { get { return _textPercentage; } }
    int _numberOfLettersToShow = 0;
    public int NumberOfLettersToShow { set { _numberOfLettersToShow = value; } get { return _numberOfLettersToShow; } }

    public void StartTyper()
    {
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
    }

    public void UpdateTextPrinter()
    {

    }
}
