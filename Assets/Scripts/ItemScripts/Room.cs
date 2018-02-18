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
    float _fadeSpeed = 3.5f;

    void Awake()
    {
        _sceneName = gameObject.name.Replace("(Clone)", "");
        foreach (SpriteRenderer renderer in this.GetComponentsInChildren<SpriteRenderer>())
        {
            _renderableObjs.Add(renderer);
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

    public IEnumerator fadeRoomOutRoutine()
    {
        bool lerpStarted = false;
        while (true)
        {
            Color currentAlpha = Color.clear;
            //MOVE CODE TO SEPARATE COROUTINE, CALL FORLOOP ONCE
            foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(fadeRoomInLerp(renderer, Color.clear));
            }
            lerpStarted = true;
            currentAlpha = GetComponentsInChildren<SpriteRenderer>()[0].color;

            if (currentAlpha.a <= .205f)
            {
                foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.color = Color.clear;
                }
                gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    public void fadeRoomIn()
    {

    }
    
    public IEnumerator fadeRoomInLerp(SpriteRenderer renderer, Color fadeToColor)
    {
        renderer.color = Color.Lerp(renderer.color, fadeToColor, _fadeSpeed * Time.deltaTime / 1);
        yield return null;
    }

    public IEnumerator fadeRoomInRoutine()
    {
        bool lerpStarted = false;
        while (true)
        {
            Color currentAlpha = Color.clear;
            
            foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(fadeRoomInLerp(renderer, Color.white));
            }
            lerpStarted = true;
            currentAlpha = GetComponentsInChildren<SpriteRenderer>()[0].color;

            if (currentAlpha.a >= .995f)
            {
                foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.color = Color.white;
                }
                PlayerControllerScript.Instance().gameObject.GetComponent<BoxCollider2D>().enabled = true;
                yield break;
            }
            yield return null;
        }
    }

    public void setRoomObjOpacity(float x)
    {
        foreach (SpriteRenderer renderer in this.GetComponentsInChildren<SpriteRenderer>())
        {
            Color alpha = new Color();
            alpha.a = x;
            renderer.color = alpha;
        }
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
