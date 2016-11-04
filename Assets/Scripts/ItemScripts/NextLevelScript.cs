using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevelScript : Interactable
{
    [SerializeField] string _nextLevel = "Default";
    [SerializeField] string _nextSpawnName = "Default";
    [SerializeField] float _xPos;
    [SerializeField] float _yPos;

    void Start()
    {
        _saveable = false;
    }

    public override void onInteract()
    {
        if(_canInteract)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().LoadLevel(_nextLevel, new Vector2(_xPos, _yPos));
        }
    }  
}
