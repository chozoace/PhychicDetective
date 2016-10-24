using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EvidenceMenuPage : NotebookMenuPage
{
    public List<Collectable> _notebookEntries = new List<Collectable>();
    [SerializeField] List<GameObject> _notebookEntryFields;
	
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
}
