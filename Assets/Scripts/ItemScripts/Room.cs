using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Interactable> _roomContents = new List<Interactable>();

    void Start()
    {

    }

    void LoadRoom()
    {

    }

	public void SaveData()
    {
        foreach(Interactable interactable in _roomContents)
        {
            interactable.SaveData();
        }
    }

    public void LoadData()
    {
        foreach (Interactable interactable in _roomContents)
        {
            interactable.LoadData();
        }
    }
}
