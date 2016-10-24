using UnityEngine;
using System.Collections;

public class PauseState : GameState
{
    NotebookController _notebookController;

    public override void Exit()
    {
        GameController.Instance()._notebookMenu.gameObject.SetActive(false);
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "PauseState";
            _notebookController = NotebookController.Instance();
        }
        GameController.Instance()._notebookMenu.gameObject.SetActive(true);
        base.Enter();
    }

    public override void UpdateState()
    {
        _notebookController.UpdateNotebook();
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameController.Instance().ChangeGameState(GameState._overworldState);
        }
        base.UpdateState();
    }
}
