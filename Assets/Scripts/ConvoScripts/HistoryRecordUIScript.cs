using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryRecordUIScript : MonoBehaviour
{
    Text _speakerTextBox;
    Text _speechTextBox;

    public void init()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            if (obj.gameObject.name == "SpeakerText")
                _speakerTextBox = obj.gameObject.GetComponent<Text>();
            else if(obj.gameObject.name == "SpeechText")
                _speechTextBox = obj.gameObject.GetComponent<Text>();
        }
    }

	public void setText(string speech, string speaker)
    {
        _speechTextBox.text = speech;
        _speakerTextBox.text = speaker;
    }
}
