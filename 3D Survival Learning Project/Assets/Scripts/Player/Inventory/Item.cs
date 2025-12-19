using System;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemData", menuName = "ScriptableObjects/Item", order = 0)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public Usable usable;
    public Tool tool;
}

[Serializable]
public class Usable
{
    public bool isUsable;
    public float healthAmount;
    public float hungerAmount;
    public float energyAmount;
}

[Serializable]
public class Tool
{
    public bool isTool;
    public float effectiveness;
    public float durability; 

    public enum ToolType
    {
        Axe,
        Pickaxe,
        Weapon
    }

    public ToolType type;

}

