using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUIScript : MonoBehaviour
{
    Text _titleObj;
    Text _descriptionObj;
    Image _imageObj;
    ItemDatabase _itemDatabase;

    void Awake()
    {
        _itemDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
        _titleObj = transform.Find("Title").GetComponent<Text>();
        _descriptionObj = transform.Find("Description").GetComponent<Text>();
        _imageObj = transform.Find("ItemImage").GetComponent<Image>();
    }

    public void setItem(int id)
    {
        if(!_itemDatabase)
            _itemDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();

        Collectable item = _itemDatabase.FindCollectableWithId(id);
        Debug.Log("item: " + item.Name);

        _titleObj.text = item.Name;
        _imageObj.sprite = Resources.Load<Sprite>("Sprites/" + item.Sprite);
        _descriptionObj.text = item.PickupDescription;
    }
}
