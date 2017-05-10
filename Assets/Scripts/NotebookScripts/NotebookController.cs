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
    [SerializeField] ClueMenuPage _cluePage;
    [SerializeField] List<NotebookMenuPage> _pages = new List<NotebookMenuPage>();
    NotebookMenuPage _currentPage;
    int _currentPageIndex = 0;
    int _maxPages;
    public NotebookMenuPage CurrentPage { get { return _currentPage; } }
    List<Collectable> _notebookItems = new List<Collectable>();
    public GameObject _notebookMenu;

    public List<Collectable> GetNotebookItems { get { return _notebookItems; } }
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        _currentPage = _evidencePage;
        _currentPage = _pages[0];
        _currentPageIndex = 0;
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
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
            throw new System.ArgumentException("Notebook Controller instance is null");
        }
    }

    public void StartNotebook()
    {
        _currentPage.ExitPage();
        _currentPage = _evidencePage;
        _currentPage.EnterPage();
    }

    public void SwitchPage(int pageIncrement)
    {
        _currentPageIndex = Mathf.Clamp(_currentPageIndex + pageIncrement, 0, _pages.Count - 1);
        if (_currentPage.GetPageIndex != _currentPageIndex)
        {
            _currentPage.ExitPage();
            _currentPage = _pages[_currentPageIndex];
            _currentPage.EnterPage();
        }
    }

    public void AddEntry(Collectable entry, string entryType)
    {
         _notebookItems.Add(entry);
        foreach(NotebookMenuPage page in _pages)
        {
            if (entryType == page._pageName && !page.PageContains(entry.ID))
            {
                _evidencePage.AddEntry(entry);
                break;
            }
        }
    }

    public bool NotebookContains(int id, string type)
    {
        bool result = false;
        //should check notebookController list instead
        foreach (NotebookMenuPage page in _pages)
        {
            if (type == page._pageName)
            {
                result = page.PageContains(id);
                break;
            }
        }
        return result;
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (!File.Exists(Application.persistentDataPath + "/notebookInfo.dat"))
            file = File.Open(Application.persistentDataPath + "/notebookInfo.dat", FileMode.Create);
        else
            file = File.Open(Application.persistentDataPath + "/notebookInfo.dat", FileMode.Open);

        NotebookSerialize ns = new NotebookSerialize();
        ns.NotebookItemsToSerialize = _notebookItems;

        bf.Serialize(file, ns);
        file.Close();
        //Debug.Log("File Path: " + Application.persistentDataPath);
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
            file.Close();
        }
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
