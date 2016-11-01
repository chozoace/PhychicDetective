using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NotebookController : MonoBehaviour
{
    static NotebookController _instance = null;
    [SerializeField] EvidenceMenuPage _evidencePage;
    [SerializeField] ProfileMenuPage _profilePage;
    NotebookMenuPage _currentPage;
    List<Collectable> _notebookItems = new List<Collectable>();

    public List<Collectable> GetNotebookItems { get { return _notebookItems; } }
    //currentPage
    //list of evidence
    //list of clues
    //list of profiles
    //database of all collectables
    //progress notes unlocked after certain checkpoints
    void Start()
    {
        _currentPage = _evidencePage;
        //LoadData();
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

    public void StartNotebook()
    {
        _currentPage.ExitPage();
        _currentPage = _evidencePage;
        _currentPage.EnterPage();
    }

    public void SwitchPage()
    {
        switch(_currentPage._pageName)
        {
            case "Evidence":
                _currentPage.ExitPage();
                _currentPage = _profilePage;
                _currentPage.EnterPage();
                break;
            case "Clue":
                break;
            case "Profile":
                _currentPage.ExitPage();
                _currentPage = _evidencePage;
                _currentPage.EnterPage();
                break;
            case "ProgressNote":
                break;
        }
    }

    public void AddEntry(Collectable entry, string entryType)
    {
        Debug.Log("entryType is " + entryType);
         _notebookItems.Add(entry);
        switch(entryType)
        {
            case "Evidence":
                if(!_evidencePage.PageContains(entry.ID))
                    _evidencePage.AddEntry(entry);
                break;
            case "Clue":
                break;
            case "Profile":
                if(!_profilePage.PageContains(entry.ID))
                    _profilePage.AddEntry(entry);
                break;
            case "ProgressNote":
                break;
        }
    }

    public bool NotebookContains(int id, string type)
    {
        bool result = false;
        //should check notebookController list instead
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

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/notebookInfo.dat"))
            file = File.Open(Application.persistentDataPath + "/notebookInfo.dat", FileMode.Create);
        else
            file = File.Open(Application.persistentDataPath + "/notebookInfo.dat", FileMode.Open);

        NotebookSerialize ns = new NotebookSerialize();
        ns.NotebookItemsToSerialize = _notebookItems;

        bf.Serialize(file, ns);
        file.Close();
        Debug.Log("File Path: " + Application.persistentDataPath);
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/notebookInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/notebookInfo.dat", FileMode.Open);
            NotebookSerialize ns = (NotebookSerialize)bf.Deserialize(file);

            foreach(Collectable item in ns.NotebookItemsToSerialize)
            {
                Debug.Log(item.Name);
                AddEntry(item, item.Type);
            }
            Debug.Log("info loaded");
            file.Close();
        }
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

[System.Serializable]
public class NotebookSerialize
{
    List<Collectable> notebookItems;
    public List<Collectable> NotebookItemsToSerialize { get { return notebookItems; } set { notebookItems = value; } }

    public NotebookSerialize()
    {

    }
}
