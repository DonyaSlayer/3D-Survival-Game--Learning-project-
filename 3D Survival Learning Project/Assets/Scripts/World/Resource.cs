using UnityEngine;
using System;

public class Resource : MonoBehaviour
{
    [Header("References")]
    public Item resourceItem;

    public float durability;
    public bool destroySelf;
    public bool getItemEveryHit;
    public Tool.ToolType toolType;
    public int minCount;
    public int maxCount;

    public event Action onGetResources;
    public event Action onHit;

    public void TryHit(Tool tool, Inventory inventory)
    {
        if (tool.type == toolType)
        {
            durability -= tool.effectiveness;
            onHit?.Invoke();

            if (durability <= 0)
            {
                GetResources(inventory);
                if (destroySelf) Destroy(gameObject);
            }
            else if(getItemEveryHit) 
            {
                GetResources(inventory);
            }
        }
    }

    private void GetResources(Inventory inventory)
    {
        inventory.AddItemCount(resourceItem, UnityEngine.Random.Range(minCount, maxCount));
        onGetResources?.Invoke();
    }
}
