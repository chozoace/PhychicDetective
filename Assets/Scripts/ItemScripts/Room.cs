using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Interactable> _roomContents = new List<Interactable>();
    List<SpriteRenderer> _renderableObjs = new List<SpriteRenderer>();
    private string _sceneName;
    public string GetRoomSceneName { get { return _sceneName; } }
    [SerializeField] Vector2 _roomPos;

    void Awake()
    {
        _sceneName = gameObject.name.Replace("(Clone)", "");
        foreach (SpriteRenderer renderer in this.GetComponentsInChildren<SpriteRenderer>())
        {

        }
       //gameObject.transform.position = _roomPos;
        DontDestroyOnLoad(this);
    }

    void LoadRoom()
    {

    }

    public void fadeRoomOut()
    {

    }

    public void fadeRoomIn()
    {

    }

	public void SaveData()
    {
        foreach(Interactable interactable in _roomContents)
        {
            interactable.SaveData();
        }
    }

    public void LoadData()
    {
        foreach (Interactable interactable in _roomContents)
        {
            interactable.LoadData();
        }
    }
}
