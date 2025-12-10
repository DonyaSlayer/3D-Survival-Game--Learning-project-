using UnityEngine;

[CreateAssetMenu (fileName = "ItemData", menuName = "ScriptableObjects/Item", order = 0)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
}
