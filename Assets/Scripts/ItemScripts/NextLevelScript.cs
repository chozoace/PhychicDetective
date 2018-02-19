using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevelScript : Interactable
{
    [SerializeField] string _nextLevel = "Default";
    [SerializeField] string _nextSpawnName = "Default";
    [SerializeField] float _xPos;
    [SerializeField] float _yPos;

    [SerializeField] float _destXPos;
    [SerializeField] float _destYPos;

    [SerializeField] int _xDirection = 1;
    [SerializeField] int _yDirection = 1;

    void Start()
    {
        _saveable = false;
    }

    public override void onInteract()
    {
        if(_canInteract)
        {
            PlayerControllerScript.Instance().CollidingInteractable = null;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().ChangeRooms(_nextLevel, new Vector2(_destXPos, _destYPos), new Vector2(_xDirection, _yDirection));

            //GameController.Instance().ChangeGameState(GameState._levelChangeState);
            //PlayerControllerScript.Instance().CollidingInteractable = null;
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().EndScene(_nextLevel, new Vector2(_xPos, _yPos));
        }
    }  
}
