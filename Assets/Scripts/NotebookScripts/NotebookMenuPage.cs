using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NotebookMenuPage : MonoBehaviour
{
    public List<Collectable> _notebookEntries = new List<Collectable>();
    [SerializeField] protected List<GameObject> _notebookEntryFields;

    public string _pageName = "Default";
    protected bool _created = false;
    protected GameObject _leftPage;
    protected int _currentIndex = 0;
    protected GameObject _cursor;
    NotebookController _notebook;

    void Start ()
    {
        _notebook = GameObject.FindGameObjectWithTag("Player").GetComponent<NotebookController>();
	}

    public virtual void EnterPage()
    {
        if (!_created)
        {
            _cursor = transform.Find("Cursor").gameObject;
        }
        this.transform.parent.gameObject.SetActive(true);
        _currentIndex = 0;
        LoadPageInfo();
    }

    public virtual void AddEntry(Collectable entry)
    {
        _notebookEntries.Add(entry);
        foreach (GameObject entryField in _notebookEntryFields)
        {
            if (entryField.GetComponent<Text>().text == "")
            {
                entryField.GetComponent<Text>().text = entry.Name;
                break;
            }
        }
    }

    public virtual void LoadPageInfo()
    {
        Vector2 v = _cursor.GetComponent<RectTransform>().position;
        v.y = _notebookEntryFields[_currentIndex].GetComponent<RectTransform>().position.y;
        _cursor.GetComponent<RectTransform>().position = v;

        //load info into left page
        if (_currentIndex < _notebookEntries.Count)
        {
            _leftPage.transform.Find("ImagePanel").GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Sprites/" + _notebookEntries[_currentIndex].Sprite);
            _leftPage.transform.Find("ItemName").GetComponent<Text>().text =
                _notebookEntries[_currentIndex].Name;
            _leftPage.transform.Find("ItemDescription").GetComponent<Text>().text =
                _notebookEntries[_currentIndex].Description;
        }
        else
        {
            _leftPage.transform.Find("ImagePanel").GetComponent<Image>().sprite = null;
            _leftPage.transform.Find("ItemName").GetComponent<Text>().text = "";
            _leftPage.transform.Find("ItemDescription").GetComponent<Text>().text = "";
        }
    }

    public virtual bool PageContains(int id)
    {
        foreach (Collectable collectable in _notebookEntries)
        {
            if (id == collectable.ID)
                return true;
        }
        return false;
    }

    public virtual void ExitPage()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
	
	public virtual void UpdatePage()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                LoadPageInfo();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_currentIndex < _notebookEntryFields.Count - 1)
            {
                _currentIndex++;
                LoadPageInfo();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _notebook.SwitchPage();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(GameController.Instance().CurrentGameState.StateName == "ConversationState")
            {
                //send info to convocontroller
                ConversationController.Instance().PresentEvidence(_notebookEntries[_currentIndex].ID);
                GameState._pauseState.Exit();
                GameState._conversationState.NotebookControl = false;
            }
        }
    }
}
