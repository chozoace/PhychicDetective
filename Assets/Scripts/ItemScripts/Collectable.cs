using UnityEngine;
using System.Collections;

public class Collectable
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public Collectable(int id, string name, string type)
    {
        this.ID = id;
        this.Name = name;
        this.Type = type;
    }

    public Collectable()
    {

    }
}
