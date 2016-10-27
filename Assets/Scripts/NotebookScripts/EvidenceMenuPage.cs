using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EvidenceMenuPage : NotebookMenuPage
{
    public List<Collectable> _notebookEntries = new List<Collectable>();
    [SerializeField] List<GameObject> _notebookEntryFields;
    //pointer object
    GameObject _cursor;
    //currently pointed index
    int _currentIndex = 0;
    //ref to left page

    void Start()
    {
        _cursor = transform.Find("Cursor").gameObject;
    }

    public override void EnterPage()
    {
        _currentIndex = 0;
    }

    public void AddEntry(Collectable entry)
    {
        //look in database to get info?
        _notebookEntries.Add(entry);
        foreach(GameObject entryField in _notebookEntryFields)
        {
            if (entryField.GetComponent<Text>().text == "")
            {
                entryField.GetComponent<Text>().text = entry.Name;
                break;
            }
        }
    }

    public bool PageContains(int id)
    {
        foreach (Collectable collectable in _notebookEntries)
        {
            if (id == collectable.ID)
                return true;
        }
        return false;
    }

    void LoadPageInfo()
    {
        Vector2 v = _cursor.GetComponent<RectTransform>().position;
        v.y = _notebookEntryFields[_currentIndex].GetComponent<RectTransform>().position.y;
        _cursor.GetComponent<RectTransform>().position = v;

        //load info into left page
    }

    public override void UpdatePage()
    {
        Debug.Log("current Index: " + _notebookEntryFields.Count);
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                LoadPageInfo();
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            if (_currentIndex < _notebookEntryFields.Count - 1)
            {
                _currentIndex++;
                LoadPageInfo();
            }
        }
    }
}
