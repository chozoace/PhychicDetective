using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuButton : MonoBehaviour, IMenuItem
{
    public void execute()
    {
        Debug.Log("Saving");
    }
}
