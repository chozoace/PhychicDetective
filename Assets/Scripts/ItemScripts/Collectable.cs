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

    public Collectable(int id, string name, string type, string sprite, string description, string comment)
    {
        this.ID = id;
        this.Name = name;
        this.Type = type;
        this.Sprite = sprite;
        this.Description = description;
        this.Comment = comment;
    }

    public Collectable()
    {

    }
}
