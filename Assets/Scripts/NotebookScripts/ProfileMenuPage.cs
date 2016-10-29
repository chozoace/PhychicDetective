using UnityEngine;
using System.Collections;

public class ProfileMenuPage : NotebookMenuPage
{
    public override void EnterPage()
    {
        if (!_created)
        {
            _pageName = "Profile";
            _leftPage = transform.parent.Find("ProfileLeft").gameObject;
        }
        base.EnterPage();
    }

}
