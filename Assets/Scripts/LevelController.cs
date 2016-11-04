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

	// Use this for initialization
	void Start ()
    {
	
	}
	
    public void LoadLevel(string newLevel, Vector2 spawnVector)
    {
        //add fade in and out
        SceneManager.LoadScene(newLevel, LoadSceneMode.Single);
        //Debug.Log("Spawn Name: " + spawnName);
        GameObject.FindGameObjectWithTag("PlayerData").transform.position = spawnVector;
        foreach(Transform child in GameObject.FindGameObjectWithTag("PlayerData").transform)
        {
            Vector3 v = new Vector3(spawnVector.x, spawnVector.y, child.position.z);
            child.position = v;
        }
    }

	// Update is called once per frame
	void Update ()
    {
	
	}
}
