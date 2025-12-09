using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [Header ("Icon UI")]
    public Image icon;
    public TMP_Text countText;
    public GameObject selection;

    [HideInInspector] public Inventory inventory;

    public void RefreshCell(Item item, int count)
    {
        if(item != null)
        {
            icon.enabled = true;
            icon.sprite = item.itemIcon;

            if(count >1)
            {
                countText.text = count.ToString();
            }
            else
            {
                countText.text = "";
            }
        }
        else
        {
            icon.enabled = false;
            icon.sprite = null;
            countText.text = "";
        } 
    }
}
