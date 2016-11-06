using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelController : MonoBehaviour
{
    //stores level info
    //load level
    //save level
    //list of interactables
    //current level/room
    GameObject _blackScreen;
    Color _currentAlphaColor;
    [SerializeField] float _fadeSpeed = 5f;
    bool _startScene = false;

    void Start ()
    {
        
    }

    public void EndScene(string newLevel, Vector2 spawnVector)
    {
        _blackScreen = GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("BlackScreen").gameObject;
        Vector3 v = GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("Main Camera").position;
        v.z = 0;
        GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("BlackScreen").position = v;
        _currentAlphaColor = _blackScreen.GetComponent<SpriteRenderer>().color;
        _startScene = false;
        StartCoroutine(EndSceneRoutine(newLevel,spawnVector));
    }

    void FadeToBlack()
    {
        _blackScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(_blackScreen.GetComponent<SpriteRenderer>().color, Color.black, _fadeSpeed * Time.deltaTime);
    }

    void FadeToClear()
    {
        _blackScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(_blackScreen.GetComponent<SpriteRenderer>().color, Color.clear, _fadeSpeed * Time.deltaTime);
    }

    public IEnumerator EndSceneRoutine(string newLevel, Vector2 spawnVector)
    {
        while(true)
        {
            FadeToBlack();

            if(_blackScreen.GetComponent<SpriteRenderer>().color.a >= .95f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = Color.black;
                LoadLevel(newLevel, spawnVector);
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public IEnumerator StartSceneRoutine()
    {
        yield return new WaitForSeconds(.05f);
        while (true)
        {
            FadeToClear();

            if (_blackScreen.GetComponent<SpriteRenderer>().color.a <= .05f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = Color.clear;
                GameController.Instance().ChangeGameState(GameState._overworldState);
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void LoadLevel(string newLevel, Vector2 spawnVector)
    {
        //Debug.Log("Changing level");
        SceneManager.LoadScene(newLevel, LoadSceneMode.Single);
        GameObject.FindGameObjectWithTag("PlayerData").transform.position = spawnVector;
        foreach(Transform child in GameObject.FindGameObjectWithTag("PlayerData").transform)
        {
            Vector3 v = new Vector3(spawnVector.x, spawnVector.y, child.position.z);
            child.position = v;
        }

        StartCoroutine("StartSceneRoutine");
    }

	void Update ()
    {
	    //if(_startScene)

	}
}
