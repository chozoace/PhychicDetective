using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    JsonData itemData;
    List<Collectable> database = new List<Collectable>();

    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Collectables.json"));
        ConstructItemDatabase();
    }

    void ConstructItemDatabase()
    {
        for(int i = 0; i < itemData.Count; i++)
        {
            //add newly generated item data from json
            database.Add(new Collectable((int)itemData[i]["id"], itemData[i]["name"].ToString(), itemData[i]["type"].ToString()));
        }
    }

    public Collectable FindCollectableWithId(int id)
    {
        return database[id];
    }
	
}
