using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotebookController : MonoBehaviour
{
    static NotebookController _instance = null;
    [SerializeField] EvidenceMenuPage _evidencePage;
    NotebookMenuPage _currentPage;
    
    //currentPage
    //list of evidence
    //list of clues
    //list of profiles
    //database of all collectables
    //progress notes unlocked after certain checkpoints
    void Start()
    {
        _currentPage = _evidencePage;
    }

    public static NotebookController Instance()
    {
        if (_instance)
        {
            return _instance;
        }
        else
        {
            throw new System.ArgumentException("ConversationController instance is null");
        }
    }

    public void AddEntry(Collectable entry, string entryType)
    {
        Debug.Log("entryType is " + entryType);
        switch(entryType)
        {
            case "Evidence":
                _evidencePage.AddEntry(entry);
                _currentPage = _evidencePage;
                break;
            case "Clue":
                break;
            case "Profile":
                break;
            case "ProgressNote":
                break;
        }
    }

    public bool NotebookContains(int id, string type)
    {
        bool result = false;
        switch (type)
        {
            case "Evidence":
                result = _evidencePage.PageContains(id);
                break;
            case "Clue":
                break;
            case "Profile":
                break;
            case "ProgressNote":
                break;
        }
        return result;
    }

    void Awake()
    {
        _instance = this;
    }

    
    public void UpdateNotebook()
    {
        //selectively update a given page?
        _currentPage.UpdatePage();
    }
}
