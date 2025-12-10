using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    public InventoryCell[] inventoryCells;
    public int[] itemCount;

    private void Start()
    {
        Refresh();
    }

    public void AddItem(Item newItem)
    {
        bool haveItem = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == newItem)
            {
                haveItem = true;
                itemCount[i]++;
                break;
            }
        }
        if (haveItem == false) 
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = newItem;
                    itemCount[i] = 1;

                    break;
                }
             }
        }

        Refresh(); 
    }

    public void ClearSlot(int index) // analogy for ItemDrop from course
    {
        itemCount[index]--;
        if (itemCount[index] == 0)
        {
            items[index] = null;
        }
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < inventoryCells.Length; i++)
        {
            inventoryCells[i].RefreshCell(items[i], itemCount[i]);
        }
    }
}
