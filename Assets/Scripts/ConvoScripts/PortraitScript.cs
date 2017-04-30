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
	}

    public void ActivatePortrait(string portraitName, string speakerName)
    {

        this.GetComponent<SpriteRenderer>().enabled = true;
        _anim.runtimeAnimatorController = Resources.Load("PortraitAnimControllers/" + speakerName + "PortraitAnim") as RuntimeAnimatorController;
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
