using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EvidenceMenuPage : NotebookMenuPage
{
    public override void EnterPage()
    {
        if (!_created)
        {
            _pageName = "Evidence";
            _leftPage = transform.parent.Find("EvidenceLeft").gameObject;
        }
        base.EnterPage();
    }

    public override void LoadPageInfo()
    {
        base.LoadPageInfo();
    }
}
