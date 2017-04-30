using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueMenuPage : NotebookMenuPage
{
    public override void EnterPage()
    {
        if (!_created)
        {
            _pageName = "Clue";
            _leftPage = transform.parent.Find("ClueLeft").gameObject;
        }
        base.EnterPage();
    }
}
