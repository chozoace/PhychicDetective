using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitScript : MonoBehaviour
{
    public enum PortraitEmotion { Neutral, Happy, Sad, Surprised, Angry, Neutral2};
    public PortraitEmotion _currentEmotion = PortraitEmotion.Neutral;

	void Start ()
    {
		
	}

    public void ActivatePortrait(string portraitName)
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
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
