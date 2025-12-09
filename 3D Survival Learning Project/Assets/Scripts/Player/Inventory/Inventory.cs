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

    public void Refresh()
    {
        for (int i = 0; i < inventoryCells.Length; i++)
        {
            inventoryCells[i].RefreshCell(items[i], itemCount[i]);
        }
    }
}
