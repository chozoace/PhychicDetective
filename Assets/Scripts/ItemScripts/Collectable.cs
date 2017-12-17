using UnityEngine;
using System.Collections;

[System.Serializable]
public class Collectable
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Sprite { get; set; }
    public string Description { get; set; }
    public string Comment { get; set; }
    public string PickupDescription { get; set; }

    public Collectable(int id, string name, string type, string sprite, string description, string comment, string pickupDescription)
    {
        ID = id;
        Name = name;
        Type = type;
        Sprite = sprite;
        Description = description;
        Comment = comment;
        PickupDescription = pickupDescription;
    }

    public Collectable()
    {

    }
}
