using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitScript : MonoBehaviour
{
    public enum PortraitEmotion { Neutral, Happy, Sad, Surprised, Angry, Neutral2};
    public PortraitEmotion _currentEmotion = PortraitEmotion.Neutral;
    Animator _anim;

	void Awake ()
    {
       _anim = GetComponent<Animator>();
        _anim.SetInteger("CurrentEmotionIndex", 0);
    }

    public void ActivatePortrait(string portraitName, string speakerName, string emotion)
    {
        switch(emotion)
        {
            case "Neutral":
                _currentEmotion = PortraitEmotion.Neutral;
                break;
            case "Happy":
                _currentEmotion = PortraitEmotion.Happy;
                break;
            case "Sad":
                _currentEmotion = PortraitEmotion.Sad;
                break;
            case "Surpised":
                _currentEmotion = PortraitEmotion.Surprised;
                break;
            case "Angry":
                _currentEmotion = PortraitEmotion.Angry;
                break;
            case "Neutral2":
                _currentEmotion = PortraitEmotion.Neutral2;
                break;
        }

        this.GetComponent<SpriteRenderer>().enabled = true;
        _anim.runtimeAnimatorController = Resources.Load("PortraitAnimControllers/" + speakerName + "PortraitAnim") as RuntimeAnimatorController;
        _anim.SetInteger("CurrentEmotionIndex", (int)_currentEmotion);
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load(portraitName, typeof(Sprite)) as Sprite;
    }

    public void StartPortraitAnimation()
    {

    }

    public void ChangePortraitEmotion()
    {

    } 

    public void DisablePortrait()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
	
	void Update ()
    {
		
	}
}
