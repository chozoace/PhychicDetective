using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPauseState : GameState
{
    SettingsPauseController _settingsPauseController;
    GameObject _settingsPauseMenuObj;

    public void setPauseMenuObj(GameObject obj)
    {
        _settingsPauseMenuObj = obj;
        _settingsPauseMenuObj.SetActive(false);
    }

    public override void Exit()
    {
        _settingsPauseMenuObj.SetActive(false);
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "SettingsPauseState";
            _settingsPauseController = GameController.Instance().GetComponent<SettingsPauseController>();
        }
        _settingsPauseMenuObj.SetActive(true);
        _settingsPauseController.StartController();
        base.Enter();
    }

    public override void UpdateState()
    {
        _settingsPauseController.UpdateMenu();
    }
}
