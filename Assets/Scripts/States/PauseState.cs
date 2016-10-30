using UnityEngine;
using System.Collections;

public class PauseState : GameState
{
    NotebookController _notebookController;

    public override void Exit()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>()._notebookMenu.gameObject.SetActive(false);
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "PauseState";
            _notebookController = NotebookController.Instance();
        }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>()._notebookMenu.gameObject.SetActive(true);
        _notebookController.StartNotebook();

        base.Enter();
    }

    public override void UpdateState()
    {
        _notebookController.UpdateNotebook();
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._overworldState);
        }
        base.UpdateState();
    }
}
