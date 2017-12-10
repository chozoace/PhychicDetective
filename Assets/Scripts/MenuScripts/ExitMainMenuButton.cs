using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ExitMainMenuButton : MonoBehaviour, IMenuItem
{
    public void execute()
    {
        Debug.Log("exiting");
        Application.Quit();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
