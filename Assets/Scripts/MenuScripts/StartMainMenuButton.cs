using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainMenuButton : MonoBehaviour, IMenuItem
{
    public void execute()
    {
        Debug.Log("starting");
        MainMenuController.Instance().startGame();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
